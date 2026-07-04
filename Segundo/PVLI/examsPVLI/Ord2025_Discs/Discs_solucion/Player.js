import Disc from "./Disc.js";

export default class Player extends Phaser.Physics.Arcade.Sprite {
	constructor(scene, x, y) {
		super(scene, x, y, 'player');

		// Añadir manualmente el player con física (collider)
		scene.add.existing(this);
		scene.physics.add.existing(this);

		this.setOrigin(0.5);

		// Cuerpo 2/3 del tamaño del sprite
		this.setSize(this.width * 2 / 3, this.height * 2 / 3);
		this.setOffset(this.width / 6, this.height / 6);

		this.speed = 80; // speed, multiplicará al vector de mov.
		this.facing = 'right'; // dirección por defecto

		// Teclas que tomará en cuenta la clase Player
		this.keys = scene.input.keyboard.addKeys('W,A,S,D,SPACE');
		this.disc = null;

		// Sistema vidas y color
		this.lives = 3;
		this.invulnerable = false;
		this.canMove = true;
		this.updateColor();

		// Anim
		this.play('player_idle');
	}

	update() {
		if (!this.canMove) return;

		let dx = 0;
		let dy = 0;

		// Si la tecla W se mantiene pulsada, o no
		if (this.keys.W.isDown) dy = -1;
		else if (this.keys.S.isDown) dy = 1;

		// Lo mismo con el eje X pero cambiando
		// el valor de facing para el flip
		if (this.keys.A.isDown) {
			dx = -1;
			this.facing = 'left';
		} else if (this.keys.D.isDown) {
			dx = 1;
			this.facing = 'right';
		}

		// Vector normalizado para la velocidad diagonal
		let velocity = new Phaser.Math.Vector2(dx, dy);
		velocity.normalize().scale(this.speed);
		// Vector de movimiento
		this.setVelocity(velocity.x, velocity.y);

		if (dx !== 0 || dy !== 0) {
			this.play('player_run', true);
		} else {
			this.play('player_idle', true);
		}

		// setFlipX es un método de Phaser (al igual que setVelocity)
		this.setFlipX(this.facing === 'left');

		// Disco
		if (Phaser.Input.Keyboard.JustDown(this.keys.SPACE)) {
			this.tryThrowDisc();
		}
		if (this.disc) {
			this.disc.update();
		}
	}

	tryThrowDisc() {
		if (!this.disc) {
			let direction = this.getThrowDirection();
			this.disc = new Disc(this.scene, this.x, this.y, direction, this);

			this.scene.physics.add.collider(this.disc, this.scene.barriers, () => {
				this.disc.bounce();
			});

			this.scene.physics.add.overlap(this.disc, this.scene.enemies, (discObj, enemyObj) => {
				if (this.disc.state === 'thrown') {
					enemyObj.loseLife();
				}
			});

		} else if (this.disc.state === 'thrown') {
			this.disc.forceReturn();
		}
	}

	getThrowDirection() {
		let vx = this.body.velocity.x;
		let vy = this.body.velocity.y;

		if (vx === 0 && vy === 0) {
			return { x: 0, y: -1 }; // parado -> siempre hacia arriba
		}

		let length = Math.sqrt(vx * vx + vy * vy);
		return { x: vx / length, y: vy / length };
	}

	updateColor() {
		const colors = { 3: 0x3399ff, 2: 0xff66cc, 1: 0xffff66 };
		this.setTint(colors[this.lives]);
	}

	loseLife() {
		if (this.invulnerable || this.lives <= 0) return;

		this.lives--;
		this.scene.sound.play('playerHit');

		if (this.lives <= 0) {
			this.setTint(0xffffff);
			this.setVelocity(0, 0);
			this.play('player_idle'); // corta la animación de correr si la había
			this.canMove = false; // pierde la capacidad de moverse

			// Muerte: sonido + transición a GameOverScene
			let looseSound = this.scene.sound.add('loose');
			looseSound.play();
			this.scene.time.delayedCall(2000, () => {
				looseSound.stop();
				this.scene.scene.start('GameOverScene', { score: this.scene.score });
			});
			return;
		}

		this.updateColor();
		this.invulnerable = true;
		this.scene.tweens.add({
			targets: this,
			alpha: 0.3,
			duration: 100,
			yoyo: true,
			repeat: 9 // 10 ciclos × 200ms ≈ 2 segundos
		});
		this.scene.time.delayedCall(2000, () => {
			this.invulnerable = false;
			this.setAlpha(1); // por si el tween no terminó en opacidad completa
		});
	}
}
