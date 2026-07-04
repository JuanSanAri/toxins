export default class GameOverScene extends Phaser.Scene {
	constructor() {
		super({ key: 'GameOverScene' });
	}

	init(data) {
		this.win = data.win; // recibe true/false desde GameScene.onVictory/onDefeat
	}

	create() {
		let centerX = this.sys.game.config.width / 2;
		let centerY = this.sys.game.config.height / 2;

		let message = this.win ? 'ALL PIGLETS\nARE SAFE' : 'GAME OVER'; // \n = salto de línea en el texto
		let color = this.win ? '#00ff00' : '#ff0000';

		this.add.text(centerX, centerY, message, {
			fontFamily: 'arcade',
			fontSize: '16px',
			color: color,
			align: 'center' // centra las dos líneas entre sí cuando hay \n
		}).setOrigin(0.5);

		this.time.delayedCall(3000, () => {
			this.scene.start('MenuScene');
		});
	}
}
