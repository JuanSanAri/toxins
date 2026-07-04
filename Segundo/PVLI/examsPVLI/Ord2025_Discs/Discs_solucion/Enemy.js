import Disc from "./Disc.js";

export default class Enemy extends Phaser.Physics.Arcade.Sprite {
	constructor(scene, x, y, speed = 50, lives = 1) {
		super(scene, x, y, 'enemy');

		scene.add.existing(this);
		scene.physics.add.existing(this);

		this.setOrigin(0.5);
		this.setSize(this.width * 2 / 3, this.height * 2 / 3);
		this.setOffset(this.width / 6, this.height / 6);

		this.speed = speed;
		this.canBounce = true; // flag de cooldown para evitar dobles rebotes en la misma colisión

		// Vidas y color
		this.lives = lives;
		this.invulnerable = false;
		// verde si 2 vidas, naranja si 1
		this.setTint(this.lives === 2 ? 0x33cc33 : 0xffa500);

		// Disco
		this.disc = null;
		this.scheduleNextThrow();

		// Anim
		this.play('enemy_run');

		// IMPORTANTE: este constructor NO llama a setRandomDirection().
		// Se llama desde fuera, en GameScene.spawnWave(), justo DESPUÉS de
		// añadir el enemigo al grupo physics.add.group(). Si se llamara aquí,
		// el grupo resetearía la velocidad a 0 al hacer this.enemies.add(enemy)
		// (ver explicación detallada abajo).
	}

	setRandomDirection() {
		let angle = Phaser.Math.Between(0, 359);
		let velocity = this.scene.physics.velocityFromAngle(angle, this.speed);
		this.direction = { x: velocity.x, y: velocity.y }; // copia propia, intocable por Arcade
		this.setVelocity(this.direction.x, this.direction.y);
	}

	reverseDirection() {
		// si está en cooldown, ignoramos el choque
		if (!this.canBounce) return;

		this.direction.x *= -1;
		this.direction.y *= -1;
		this.setVelocity(this.direction.x, this.direction.y); // sobreescribimos lo que Arcade haya tocado

		this.canBounce = false;
		this.scene.time.delayedCall(200, () => {
			this.canBounce = true;
		});
	}

	loseLife() {
		if (this.invulnerable || this.lives <= 0) return;

		this.lives--;

		if (this.lives <= 0) {
			this.scene.sound.play('enemyDeath');
			this.scene.addScore(150);
			if (this.disc) {
				this.disc.destroy();
				this.disc = null;
			}
			this.destroy();
			return;
		}

		// Si sigue vivo NO cambia de color (lo pide así el enunciado),
		// simplemente pilla invulnerabilidad durante 1s
		this.invulnerable = true;
		// No lo pide el enunciado pero da feedback visual del golpe
		this.scene.tweens.add({
			targets: this,
			alpha: 0.3,
			duration: 100,
			yoyo: true,
			repeat: 4 // 5 ciclos de ~200ms ≈ 1 segundo
		});
		this.scene.time.delayedCall(1000, () => {
			this.invulnerable = false;
			this.setAlpha(1);
		});
	}

	scheduleNextThrow() {
		let delay = Phaser.Math.Between(1500, 3000); // entre 1.5 y 3 segundos
		this.throwTimer = this.scene.time.delayedCall(delay, () => {
			this.tryThrowDisc();
			this.scheduleNextThrow(); // se reprograma a sí mismo
		});
	}

	tryThrowDisc() {
		if (this.disc) return; // ya tiene uno fuera, no lanza otro

		let player = this.scene.player;
		let dx = player.x - this.x;
		let dy = player.y - this.y;
		let length = Math.sqrt(dx * dx + dy * dy) || 1;
		let direction = { x: dx / length, y: dy / length };

		this.disc = new Disc(this.scene, this.x, this.y, direction, this, this.speed + 100);
		this.disc.setTint(0xff0000);

		this.scene.physics.add.collider(this.disc, this.scene.barriers, () => {
			this.disc.bounce();
		});

		this.scene.physics.add.overlap(this.disc, this.scene.player, () => {
			if (this.disc.state === 'thrown') {
				this.scene.player.loseLife();
			}
		});
	}

	update() {
		if (this.disc) {
			this.disc.update();
		}

		// Espeja el sprite según el sentido horizontal actual del movimiento
		if (this.body.velocity.x !== 0) {
			this.setFlipX(this.body.velocity.x > 0);
		}
	}

	destroy(fromScene) {
		if (this.throwTimer) this.throwTimer.remove(); // evita que siga intentando lanzar tras morir
		if (this.disc) this.disc.destroy();
		super.destroy(fromScene);
	}
}
