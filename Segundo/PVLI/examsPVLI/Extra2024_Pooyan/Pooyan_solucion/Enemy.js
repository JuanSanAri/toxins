export default class Wolf extends Phaser.Physics.Arcade.Sprite {
	constructor(scene, x, y) {
		super(scene, x, y, 'wolf');

		scene.add.existing(this);
		scene.physics.add.existing(this);

		this.setOrigin(0.5);
		this.body.setAllowGravity(false); // controlamos la física manualmente según el estado

		this.lives = scene.mode === 'hard' ? 2 : 1; // hard: necesita 2 flechas en el globo para morir
		this.state = 'running';
		this.hasGlobe = true;

		this.play('wolf_run');

		// tiempo aleatorio antes de inflar
		let runTime = Phaser.Math.Between(1000, 3000);
		scene.time.delayedCall(runTime, () => {
			this.startBlowing();
		});
	}

	startBlowing() {
		if (!this.active) return; // por si murió mientras corría (flecha en árbol no debería pasar, pero por si acaso)
		this.state = 'blowing';
		this.setVelocityX(0); // se detiene para inflar
		this.play('wolf_blow');

		// animationcomplete-{key}: evento de Phaser que se dispara al terminar una anim con repeat:0
		this.once('animationcomplete-wolf_blow', () => {
			this.startFalling();
		});
	}

	startFalling() {
		if (!this.active) return;
		this.state = 'falling'; // a partir de aquí es vulnerable a las flechas
		this.hasGlobe = true;
		this.play('wolf_idle'); // lobo flotando con globo hinchado
		this.body.setAllowGravity(true); // ahora sí cae por la gravedad del config
		this.setVelocityY(10); // pequeño empuje inicial hacia abajo
	}

	land() {
		if (!this.active) return;
		this.body.setAllowGravity(false);
		this.setVelocityY(0);

		if (this.hasGlobe) {
			// llegó al suelo con globo intacto -> corre hacia la derecha y sale de pantalla
			this.state = 'grounded';
			this.play('wolf_run_balloon');
			this.setFlipX(false); // ahora mira hacia la derecha
			this.setVelocityX(60);
		} else {
			// llegó al suelo sin globo (flecha en parte inferior) -> muere en el suelo
			this.state = 'dead';
			if (this.flipTimer) this.flipTimer.remove(); // para el flipX alternante si lo hubiera
			this.play('wolf_down'); // animación de muerte en el suelo
			this.setVelocityX(0);
			this.scene.time.delayedCall(1000, () => {
				this.destroy(); // desaparece tras 1s, NO suma kill (el jugador no explotó el globo)
			});
		}
	}

	popGlobe() {
		if (!this.active || !this.hasGlobe) return; // guarda doble: no procesar si ya no tiene globo

		this.hasGlobe = false;
		this.lives--;
		this.scene.sound.play('balloon');

		this.play('wolf_fall'); // lobo cayendo sin globo
		this.setVelocityY(100); // cae más rápido que con globo

		this.flipTimer = this.scene.time.addEvent({
			delay: 200,
			callback: () => { this.setFlipX(!this.flipX); }, // alterna el flip cada 200ms
			loop: true
		});

		if (this.lives <= 0) {
			this.scene.killEnemy(); // avisa a la escena: actualiza contador y comprueba victoria
		}
		// si lives > 0 (modo hard, primera flecha): sigue cayendo sin globo, puede llegar al suelo
	}

	hitBottom(arrow) {
		// flecha en la mitad inferior del sprite (donde no hay globo) -> la flecha sale disparada hacia abajo
		arrow.processed = true; // evita que el overlap la procese otra vez mientras sigue en contacto
		arrow.setAngle(-90); // rota el sprite visualmente 90 grados
		arrow.body.setSize(arrow.height, arrow.width); // Arcade Physics no rota bodies, hay que intercambiar ancho/alto a mano
		arrow.body.setAllowGravity(true);
		arrow.setVelocityX(0);
		arrow.setVelocityY(150); // sale disparada hacia abajo
	}

	update() {
		let groundY = 211; // this.scene.sys.game.config.height - 29, ajustado visualmente al fondo del escenario
		if (this.state === 'falling' && this.y >= groundY) {
			this.y = groundY; // ancla en el suelo exacto antes de llamar a land()
			this.land();
		}

		// quita una vida al jugador
		if (this.state === 'grounded' && this.x > this.scene.sys.game.config.width + 20) {
			this.scene.loseLife();
			this.destroy();
		}
	}
}