export default class MenuScene extends Phaser.Scene {
	constructor() {
		super({ key: 'MenuScene' });
	}

	create() {
		let centerX = this.sys.game.config.width / 2;
		let centerY = this.sys.game.config.height / 2;

		this.add.text(centerX, 60, 'POOYAN', {
			fontFamily: 'hadley',
			fontSize: '32px',
			color: '#01DADA',
			stroke: '#ffffff',
			strokeThickness: 4
		}).setOrigin(0.5);

		let easyBtn = this.add.text(centerX, centerY + 10, 'EASY', {
			fontFamily: 'arcade',
			fontSize: '16px',
			color: '#ffffff'
		}).setOrigin(0.5);
		easyBtn.setInteractive({ useHandCursor: true });
		easyBtn.once('pointerdown', () => {
			this.scene.start('GameScene', { mode: 'easy' }); // pasamos el modo a GameScene via init(data)
		});

		let hardBtn = this.add.text(centerX, centerY + 40, 'HARD', {
			fontFamily: 'arcade',
			fontSize: '16px',
			color: '#ffffff'
		}).setOrigin(0.5);
		hardBtn.setInteractive({ useHandCursor: true });
		hardBtn.once('pointerdown', () => {
			this.scene.start('GameScene', { mode: 'hard' }); // hard: 20 kills, lobos con 2 vidas
		});
	}
}
