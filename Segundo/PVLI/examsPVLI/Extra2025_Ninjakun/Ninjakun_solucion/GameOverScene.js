export default class GameOverScene extends Phaser.Scene {
	constructor() {
		super({ key: 'GameOverScene' });
	}

	init(data) {
		this.win = data.win;
		this.score = data.score;
		this.hiScore = data.hiScore;
		this.newRecord = data.newRecord;
	}

	create() {
		let cx = this.sys.game.config.width / 2;

		this.add.text(cx, 60, 'GAME OVER', {
			fontFamily: 'upheat', fontSize: '32px', color: '#ffffff'
		}).setOrigin(0.5);

		this.add.text(cx, 100, this.win ? 'YOU WIN' : 'YOU LOOSE', {
			fontFamily: 'upheat', fontSize: '24px', color: '#ffffff'
		}).setOrigin(0.5);

		if (this.newRecord) {
			this.add.text(cx, 160, 'NEW SCORE: ' + this.score, {
				fontFamily: 'upheat', fontSize: '16px', color: '#00ffff'
			}).setOrigin(0.5);
		} else {
			this.add.text(cx, 160, 'SCORE: ' + this.score, {
				fontFamily: 'upheat', fontSize: '16px', color: '#00ffff'
			}).setOrigin(0.5);
			this.add.text(cx, 185, 'HIGHSCORE: ' + this.hiScore, {
				fontFamily: 'upheat', fontSize: '16px', color: '#00ffff'
			}).setOrigin(0.5);
		}

		this.sound.play('gameover');

		this.time.delayedCall(3000, () => {
			this.sound.stopAll();
			this.scene.start('MenuScene');
		});
	}
}