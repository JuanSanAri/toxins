export default class MainMenu extends Phaser.Scene {
	constructor() {
		super({ key: 'MainMenu' });
	}

	create() {
		let cx = this.sys.game.config.width / 2;
		let cy = this.sys.game.config.height / 2;
		let h = this.sys.game.config.height;

		// hi score arriba (lo recuperaremos de localStorage más adelante)
		let hiScore = localStorage.getItem('hiScore') || 0;
		this.add.text(cx, 20, 'HI SCORE', {
			fontFamily: 'upheat', fontSize: '16px', color: '#ff0000'
		}).setOrigin(0.5);
		this.add.text(cx, 34, hiScore, {
			fontFamily: 'upheat', fontSize: '16px',
		}).setOrigin(0.5);

		// título principal
		this.add.text(cx, cy, 'NINJA - KUN', {
			fontFamily: 'upheat', fontSize: '36px', color: '#00ffff'
		}).setOrigin(0.5);

		// texto parpadeante abajo
		let pressText = this.add.text(cx, h - 10, 'PRESS SPACE TO CONTINUE', {
			fontFamily: 'upheat', fontSize: '18px', color: '#ff0000'
		}).setOrigin(0.5);

		this.tweens.add({
			targets: pressText,
			alpha: 0,
			duration: 500,
			yoyo: true,
			repeat: -1
		});

		this.input.keyboard.once('keydown-SPACE', () => {
			this.scene.start('GameScene');
		});
	}
}