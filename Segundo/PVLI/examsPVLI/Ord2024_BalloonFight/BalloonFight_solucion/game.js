import MenuScene from "./MenuScene.js";
import GameScene from "./GameScene.js";
import GameOver from "./GameOverScene.js";

let config = {
	type: Phaser.AUTO,
	width: 480,
	height: 320,
	pixelArt: true,
	scale: {
		autoCenter: Phaser.Scale.CENTER_HORIZONTALLY,
		mode: Phaser.Scale.FIT,
		min: { width: 480, height: 320 },
		max: { width: 960, height: 640 }
	},
	scene: [MenuScene, GameScene, GameOver], // la PRIMERA del array arranca automáticamente al cargar
	physics: {
		default: 'arcade',
		arcade: {
			gravity: { y: 80 },
			debug: false
		}
	}
};

new Phaser.Game(config);