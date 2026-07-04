export default class Player extends Phaser.Physics.Arcade.Sprite {
	constructor(scene, x, y) {
		super(scene, x, y, 'player');

		scene.add.existing(this);
		scene.physics.add.existing(this);

		this.setOrigin(0.5);
		this.body.setAllowGravity(false); // el jugador sube/baja por input, no por física

		this.minY = 55; // límite superior de movimiento
		this.maxY = 175; // límite inferior de movimiento

		this.speed = 70;
		this.keys = scene.input.keyboard.addKeys('UP,DOWN,Z');

		this.hasMeat = false; // false = dispara flecha, true = dispara carne (powerup activo)
		this.canShoot = true; // cooldown: false durante el segundo de recarga

		this.play('idle_arrow');

		// active es una propiedad nativa de Phaser.GameObjects -> cuando es false,
		// update() queda bloqueado y el body deja de procesar colisiones
		this.active = true;
	}

	update() {
		if (!this.active) return; // jugador muerto: no procesa nada

		if (this.keys.UP.isDown && this.y > this.minY) {
			this.setVelocityY(-this.speed);
		} else if (this.keys.DOWN.isDown && this.y < this.maxY) {
			this.setVelocityY(this.speed);
		} else {
			this.setVelocityY(0); // sin tecla pulsada: se detiene en seco
		}

		if (Phaser.Input.Keyboard.JustDown(this.keys.Z)) { // JustDown: solo el frame exacto de la pulsación
			this.shoot();
		}
	}

	shoot() {
		if (!this.canShoot) return; // cooldown activo: ignorar disparo

		this.canShoot = false;
		this.play('shoot'); // frame 1: animación de disparo, repeat:0 -> se queda en ese frame hasta que termine la recarga
		this.scene.sound.play('shot');

		if (this.hasMeat) {
			this.shootMeat();
		} else {
			this.shootArrow();
		}

		this.scene.time.delayedCall(1000, () => { // 1 segundo de recarga
			this.canShoot = true;
			this.hasMeat = false; // la carne es de un solo uso: vuelve a flechas tras disparar
			this.play('idle_arrow');
		});
	}

	shootArrow() {
		let arrow = this.scene.physics.add.sprite(this.x - 10, this.y + 6, 'arrow');
		arrow.isMeat = false; // propiedad custom para distinguirla de la carne en el overlap
		this.scene.projectiles.add(arrow); // añadir al grupo ANTES de configurar el body
		arrow.body.setAllowGravity(false); // DESPUÉS del add (el grupo resetea el body al añadir)
		arrow.setVelocityX(-500); // DESPUÉS del add
	}

	shootMeat() {
		let meat = this.scene.physics.add.sprite(this.x - 10, this.y, 'meat');
		meat.isMeat = true;
		this.scene.projectiles.add(meat);
		meat.setVelocity(-80, -30); // sale hacia la izquierda y hacia arriba, la gravedad del config hace la parábola
	}

	collectPowerUp() {
		this.hasMeat = true;
		this.play('idle_meat');
	}
}