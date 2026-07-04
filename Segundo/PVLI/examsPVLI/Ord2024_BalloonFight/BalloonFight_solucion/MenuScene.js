export default class MenuScene extends Phaser.Scene {
	constructor() {
		super({ key: 'MenuScene' });
	}

	create() {
		let centerX = this.sys.game.config.width / 2;
		let centerY = this.sys.game.config.height / 2;

		// setOrigin(1, 0.5): ancla el texto por su borde DERECHO (X=1) y centro vertical (Y=0.5)
		// así "BALLOON\nFIGHT" queda alineado a la derecha del punto dado
		// align: 'right' alinea las dos líneas entre sí (la más corta termina en el mismo borde derecho que la más larga)
		this.add.text(centerX + 140, 80, 'BALLOON\nFIGHT', {
			fontFamily: 'balloonfont',
			fontSize: '64px',
			color: '#ff9100',
			align: 'right'
		}).setOrigin(1, 0.5);

		let startButton = this.add.text(centerX, centerY + 50, 'Start', {
			fontFamily: 'arcade',
			fontSize: '24px',
			color: '#ffffff'
		}).setOrigin(0.5);

		startButton.setInteractive({ useHandCursor: true }); // sin setInteractive, el texto no recibe eventos de ratón
		// useHandCursor: true cambia el cursor a manita al pasar por encima

		startButton.once('pointerdown', () => { // se dispara una sola vez
			this.scene.start('gameScene');
		});
	}
}