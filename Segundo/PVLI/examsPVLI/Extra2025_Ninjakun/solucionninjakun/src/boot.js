export default class Boot extends Phaser.Scene {

	constructor() {
		super({ key: 'boot' });
	}

	preload() {
		this.load.image('bg', 'assets/bg.png');
		this.load.image('boost', 'assets/boost.png');

		this.load.spritesheet('player', 'assets/player.png', { frameWidth: 16, frameHeight: 16 });
		this.load.spritesheet('enemy', 'assets/enemy.png', { frameWidth: 16, frameHeight: 16 });
		this.load.spritesheet('shuriken', 'assets/shuriken.png', { frameWidth: 8, frameHeight: 8 });

		this.load.audio('boost_sound', 'assets/sounds/boost.mp3');
		this.load.audio('enemy_die', 'assets/sounds/enemy_die.mp3');
		this.load.audio('gameover', 'assets/sounds/gameover.mp3');
		this.load.audio('jump', 'assets/sounds/jump.mp3');
		this.load.audio('player_die', 'assets/sounds/player_die.mp3');
		this.load.audio('shoot', 'assets/sounds/shoot.mp3');
	}

	create() {
		this.anims.create({
			key: 'player_idle',
			frames: this.anims.generateFrameNumbers('player', { start: 0, end: 0 }),
			frameRate: 5,
			repeat: -1
		});

		this.anims.create({
			key: 'player_jump',
			frames: this.anims.generateFrameNumbers('player', { start: 2, end: 2 }),
			frameRate: 5,
			repeat: -1
		});

		this.anims.create({
			key: 'player_run',
			frames: this.anims.generateFrameNumbers('player', { start: 2, end: 3 }),
			frameRate: 5,
			repeat: -1
		});

		this.anims.create({
			key: 'player_shoot',
			frames: this.anims.generateFrameNumbers('player', { start: 4, end: 5 }),
			frameRate: 5,
			repeat: 0
		});

		this.anims.create({
			key: 'player_die',
			frames: this.anims.generateFrameNumbers('player', { start: 7, end: 7 }),
			frameRate: 5,
			repeat: 0
		});

		this.anims.create({
			key: 'enemy_idle',
			frames: this.anims.generateFrameNumbers('enemy', { start: 0, end: 0 }),
			frameRate: 5,
			repeat: 0,
		});

		this.anims.create({
			key: 'enemy_run',
			frames: this.anims.generateFrameNumbers('enemy', { start: 0, end: 1 }),
			frameRate: 5,
			repeat: 0,
		});

		this.anims.create({
			key: 'enemy_stun',
			frames: this.anims.generateFrameNumbers('enemy', { start: 3, end: 3 }),
			frameRate: 5,
			repeat: 0,
		});

		this.anims.create({
			key: 'enemy_die',
			frames: this.anims.generateFrameNumbers('enemy', { start: 2, end: 2 }),
			frameRate: 5,
			repeat: 0,
		});

		this.anims.create({
			key: 'shuriken_rotate',
			frames: this.anims.generateFrameNumbers('shuriken', { start: 0, end: 1 }),
			frameRate: 10,
			repeat: -1,
		});

		this.anims.create({
			key: 'shuriken_puf',
			frames: this.anims.generateFrameNumbers('shuriken', { start: 3, end: 3 }),
			frameRate: 10,
			repeat: 0,
		});
		// automáticamente a MainMenu
		this.scene.start('MainMenu');
	}
}