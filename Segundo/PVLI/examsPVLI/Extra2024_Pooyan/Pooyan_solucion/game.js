import MenuScene from "./MenuScene.js";
import GameScene from "./GameScene.js";
import GameOverScene from "./GameOverScene.js";

let config = {
	type: Phaser.AUTO,
	width: 256,
	height: 240,
	pixelArt: true,
	scale: {
		autoCenter: Phaser.Scale.CENTER_HORIZONTALLY,
		mode: Phaser.Scale.FIT,
		min: { width: 256, height: 240 },
		max: { width: 960, height: 640 }
	},
	scene: [MenuScene, GameScene, GameOverScene],
	physics: {
		default: 'arcade',
		arcade: {
			gravity: { y: 40 },
			debug: false
		}
	}
};

new Phaser.Game(config);