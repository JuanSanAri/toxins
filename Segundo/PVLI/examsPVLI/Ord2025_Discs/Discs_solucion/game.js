import GameScene from "./GameScene.js";
import MenuScene from "./MenuScene.js";
import GameOverScene from "./GameOverScene.js";

let config = {
	type: Phaser.AUTO,
	parent: 'juego',
	pixelArt: true,
	scale: {
		autoCenter: Phaser.Scale.CENTER_HORIZONTALLY,
		mode: Phaser.Scale.FIT,
		width: 500,
		height: 300,
		zoom: 1
	},
	scene: [MenuScene, GameScene, GameOverScene],
	physics: {
		default: 'arcade',
		arcade: {
			//gravity: { y: 200 },
			debug: true
		},
		checkCollision: {
			up: true,
			down: true,
			left: true,
			right: true
		}
	},
	title: "PVLI Ordinaria 25/26",
	version: "1.0.0",
	transparent: false
};

new Phaser.Game(config);
