export default class MenuScene extends Phaser.Scene {
	constructor() {
		super({ key: 'MenuScene' });
	}

	create() {
		// Título TRON en blanco
		this.add.text(250, 90, 'TRON', {
			fontFamily: 'arcade',
			fontSize: '48px',
			color: '#ffffff'
		}).setOrigin(0.5);

		// Subtítulo en verde, más pequeño
		this.add.text(250, 140, 'Deadly Discs', {
			fontFamily: 'arcade',
			fontSize: '24px',
			color: '#00ff00'
		}).setOrigin(0.5);

		// Texto parpadeante
		let pressText = this.add.text(250, 220, 'Pulsa ESPACIO para continuar', {
			fontFamily: 'arcade',
			fontSize: '16px',
			color: '#ffffff'
		}).setOrigin(0.5);

		this.tweens.add({
			targets: pressText,
			alpha: 0,
			duration: 500,
			yoyo: true,
			repeat: -1
		});

		// Al pulsar espacio, una sola vez, pasamos a la escena de juego
		this.input.keyboard.once('keydown-SPACE', () => {
			this.scene.start('GameScene');
		});
	}
}
