export default class Platform extends Phaser.GameObjects.Rectangle {
	constructor(scene, y, left, right) {
		let width = right - left;
		let centerX = left + width / 2;

		super(scene, centerX, y, width, 4, 0x000000, 0);

		scene.add.existing(this);
		scene.physics.add.existing(this, true);
		this.body.checkCollision.down = false;
		this.body.checkCollision.right = false;
		this.body.checkCollision.left = false;

		this.body.setSize(width, 4); // forzar tamaño del body explícitamente
		this.body.updateFromGameObject(); // sincroniza el body con la posición/tamaño del GameObject
	}
}