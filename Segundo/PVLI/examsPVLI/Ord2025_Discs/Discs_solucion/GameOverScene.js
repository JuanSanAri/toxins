export default class GameOverScene extends Phaser.Scene {
	constructor() {
		super({ key: 'GameOverScene' });
	}

	init(data) {
		this.finalScore = data.score || 0;
	}

	create() {
		this.add.text(250, 120, 'GAME OVER', {
			fontFamily: 'arcade',
			fontSize: '32px',
			color: '#ffffff'
		}).setOrigin(0.5);

		this.add.text(250, 160, 'Puntuación: ' + this.finalScore, {
			fontSize: '16px',
			color: '#ffffff'
		}).setOrigin(0.5);

		this.time.delayedCall(3000, () => {
			this.scene.start('MenuScene');
		});
	}
}
