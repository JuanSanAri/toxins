export default class GameOver extends Phaser.Scene {
	constructor() {
		super({ key: 'GameOver' });
	}

	init(data) {
		this.win = data.win;
		this.finalScore = data.score || 0; // || 0 por si alguien arranca la escena sin pasar datos
	}

	create() {
		let message = this.win ? 'VICTORY!' : 'GAME OVER'; // operador ternario: condición ? siTrue : siFalse
		let color = this.win ? '#00ff00' : '#ff0000';

		this.add.text(240, 120, message, {
			fontFamily: 'arcade',
			fontSize: '32px',
			color: color
		}).setOrigin(0.5);

		if (this.win) { // la puntuación solo se muestra si has ganado
			this.add.text(240, 160, 'Puntuación: ' + this.finalScore, {
				fontSize: '16px',
				color: '#ffffff'
			}).setOrigin(0.5);
		}

		this.time.delayedCall(3000, () => { // vuelta al menú después de 3s
			this.scene.start('MenuScene');
		});
	}
}
