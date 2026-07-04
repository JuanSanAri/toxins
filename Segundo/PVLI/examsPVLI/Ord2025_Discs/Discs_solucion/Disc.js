export default class Disc extends Phaser.Physics.Arcade.Sprite {
	constructor(scene, x, y, direction, owner, speed = 200) {
		super(scene, x, y, 'disc');

		scene.add.existing(this);
		scene.physics.add.existing(this);

		this.owner = owner;
		this.speed = speed;
		this.state = 'thrown'; // 'thrown' (hace daño) | 'returning' (vuelve, no hace daño)

		this.setVelocity(direction.x * this.speed, direction.y * this.speed);
		this.rotation = Math.atan2(direction.y, direction.x);
	}

	bounce() {
		if (this.state !== 'thrown') return; // ya estaba volviendo, ignoramos doble choque
		this.state = 'returning';
		this.setTexture('discOff');
	}

	forceReturn() {
		if (this.state !== 'thrown') return;
		this.state = 'returning';
		this.setTexture('discOff');
	}

	update() {
		if (this.state !== 'returning') return;

		let distance = Phaser.Math.Distance.Between(this.x, this.y, this.owner.x, this.owner.y);

		if (distance < 10) {
			this.owner.disc = null;
			this.destroy();
			return;
		}

		this.scene.physics.moveToObject(this, this.owner, this.speed);
		this.rotation = Phaser.Math.Angle.Between(this.x, this.y, this.owner.x, this.owner.y);
	}
}
