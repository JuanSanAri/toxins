export default class PowerUp extends Phaser.Physics.Arcade.Sprite {
	constructor(scene, x, y) {
		super(scene, x, y, 'meat'); // reutiliza la textura meat.png, no hace falta asset nuevo

		scene.add.existing(this);
		scene.physics.add.existing(this);

		this.body.setAllowGravity(false); // flota en su posición, no cae
	}
}