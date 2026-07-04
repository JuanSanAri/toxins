export default class Boot extends Phaser.Scene {

	constructor() {
		super({ key: 'boot' });
	}

	preload(){
		//Imagenes
		// 'bg', 'assets/bg.png'
		// 'boost', 'assets/boost.png'
		
		//Sprites
		// 'player', 'assets/player.png', {frameWidth: 16, frameHeight: 16}
		// 'enemy', 'assets/enemy.png', {frameWidth: 16, frameHeight: 16}
		// 'shuriken', 'assets/shuriken.png', {frameWidth: 8, frameHeight: 8}

		//Sonidos
		// "boost", "assets/sounds/boost.mp3"
		// "enemy_die", "assets/sounds/enemy_die.mp3"
		// "gameover", "assets/sounds/gameover.mp3"
		// "jump", "assets/sounds/jump.mp3"
		// "player_die", "assets/sounds/player_die.mp3"
		// "shoot", "assets/sounds/shoot.mp3"
	}
	
	//ANIMACIONES
	/*
		this.anims.create({
			key: 'player_idle',
			frames: this.anims.generateFrameNumbers('player', {start:0, end:0}),
			frameRate: 5,
			repeat: -1
		});

		this.anims.create({
			key: 'player_jump',
			frames: this.anims.generateFrameNumbers('player', {start:2, end:2}),
			frameRate: 5,
			repeat: -1
		});

		this.anims.create({
			key: 'player_run',
			frames: this.anims.generateFrameNumbers('player', {start:2, end:3}),
			frameRate: 5,
			repeat: -1
		});
		
		this.anims.create({
			key: 'player_shoot',
			frames: this.anims.generateFrameNumbers('player', {start:4, end:5}),
			frameRate: 5,
			repeat: 0
		});

		this.anims.create({
			key: 'player_die',
			frames: this.anims.generateFrameNumbers('player', {start:7, end:7}),
			frameRate: 5,
			repeat: 0
		});
	
		this.anims.create({
			key: 'enemy_idle',
			frames: this.anims.generateFrameNumbers('enemy', {start:0, end:0}),
			frameRate: 5,
			repeat: 0,
		});

		this.anims.create({
			key: 'enemy_run',
			frames: this.anims.generateFrameNumbers('enemy', {start:0, end:1}),
			frameRate: 5,
			repeat: 0,
		});

		this.anims.create({
			key: 'enemy_stun',
			frames: this.anims.generateFrameNumbers('enemy', {start:3, end:3}),
			frameRate: 5,
			repeat: 0,
		});

		this.anims.create({
			key: 'enemy_die',
			frames: this.anims.generateFrameNumbers('enemy', {start:2, end:2}),
			frameRate: 5,
			repeat: 0,
		});

		this.anims.create({
			key: 'shuriken_rotate',
			frames: this.anims.generateFrameNumbers('shuriken', {start:0, end:1}),
			frameRate: 10,
			repeat: -1,
		});

		this.anims.create({
			key: 'shuriken_puf',
			frames: this.anims.generateFrameNumbers('shuriken', {start:3, end:3}),
			frameRate: 10,
			repeat: 0,
		});
	*/
}