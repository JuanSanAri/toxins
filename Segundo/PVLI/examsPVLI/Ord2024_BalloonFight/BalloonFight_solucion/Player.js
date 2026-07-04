export default class Player {
	constructor(scene, x, y) {
		this.scene = scene;

		// physics.add.sprite es el atajo que hace add.existing + physics.add.existing en una sola línea
		this.doll = scene.physics.add.sprite(x, y, 'player');

		this.balloon = scene.physics.add.sprite(x, y - 12, 'balloon');
		this.balloon.body.setAllowGravity(false); // el globo no cae, lo movemos a mano
		this.balloon.body.moves = false; // Arcade Physics no calcula su posición, la damos nosotros cada frame

		this.hasBalloon = true; // flag: controla si el jugador puede volar y si puede recibir daño de los enemigos

		this.keys = scene.input.keyboard.addKeys('LEFT,RIGHT,Z');

		this.speed = 60;
		this.flySpeed = -50;
		this.facing = 'right';

		this.doll.setDragX(50); // reduce la velocidad X gradualmente al soltar las teclas

		this.doll.play('player_idle');
		this.balloon.play('balloon_normal');
	}

	update() {
		if (!this.hasBalloon) return; // sin globo: no hay input, el muñeco cae libre por gravedad

		let flying = this.keys.Z.isDown; // Z mantenida = volar

		if (flying) {
			this.doll.play('player_fly', true); // true = no reiniciar la anim si ya se estaba reproduciendo
			this.doll.setVelocityY(this.flySpeed); // empuje hacia arriba cada frame que se mantiene Z

			if (this.keys.LEFT.isDown) {
				this.doll.setVelocityX(-this.speed);
				this.facing = 'left';
			} else if (this.keys.RIGHT.isDown) {
				this.doll.setVelocityX(this.speed);
				this.facing = 'right';
			}
		} else {
			this.doll.play('player_idle', true);
			// sin Z: no tocamos velocityY (la gravedad del config actúa sola), ni velocityX (el drag frena)
		}

		this.doll.setFlipX(this.facing === 'right');
		this.balloon.setFlipX(this.facing === 'right');

		// wrap toroidal en X: sale por un lado, aparece por el otro
		let width = this.scene.sys.game.config.width;
		if (this.doll.x < 0) this.doll.x = width;
		if (this.doll.x > width) this.doll.x = 0;

		// tope duro en Y: no sale del canvas ni por arriba ni por abajo
		let height = this.scene.sys.game.config.height;
		if (this.doll.y < 20) { this.doll.y = 20; this.doll.setVelocityY(0); }
		if (this.doll.y > height) { this.doll.y = height; this.doll.setVelocityY(0); }

		// el globo sigue al muñeco cada frame (su body.moves = false, así que no se mueve solo)
		this.balloon.x = this.doll.x;
		this.balloon.y = this.doll.y - 12;
	}

	popBalloon() {
		if (!this.hasBalloon) return; // guarda: evita procesar el pop si ya no tiene globo

		this.hasBalloon = false;
		this.balloon.play('balloon_pop'); // anim de explosión del globo (frame 1 del spritesheet balloon.png)
		this.scene.time.delayedCall(300, () => { // esperamos 300ms para que se vea la anim antes de ocultar
			this.balloon.setVisible(false);
			this.balloon.body.enable = false; // desactiva el body: ya no detecta colisiones
		});

		this.doll.setTexture('dead'); // swap de textura en caliente: a partir de aquí usa dead.png en vez de player.png
		this.doll.play('player_dead'); // anim de muerte (repeat: 0, se reproduce una vez y se queda en el último frame)
		this.doll.setVelocity(0, 0); // cancela cualquier velocidad que llevara al morir
	}
}