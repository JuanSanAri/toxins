export default class Shuriken extends Phaser.Physics.Arcade.Sprite {
	constructor(scene, x, y, direction, owner) {
		super(scene, x, y, 'shuriken');

		scene.add.existing(this);
		scene.physics.add.existing(this);

		this.owner = owner; // 'player' o 'enemy', para saber a quién puede dañar
		this.speed = 120;

		this.play('shuriken_rotate');
		this.setVelocityX(direction === 'left' ? -this.speed : this.speed);

		// tiempo de vida: 2 segundos, luego desaparece con el frame puf
		this.lifeTimer = scene.time.delayedCall(2000, () => {
			this.pop();
		});
	}

	pop() {
		if (!this.active) return; // ya destruido o "popeando"
		this.setVelocity(0, 0);
		this.play('shuriken_puf');
		this.body.enable = false; // deja de colisionar mientras se ve el frame puf

		this.scene.time.delayedCall(500, () => {
			this.destroy();
		});
	}

	destroy(fromScene) {
		if (this.lifeTimer) this.lifeTimer.remove();
		super.destroy(fromScene);
	}
}