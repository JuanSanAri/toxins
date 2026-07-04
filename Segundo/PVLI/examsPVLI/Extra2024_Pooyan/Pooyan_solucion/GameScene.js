import Player from './Player.js';
import Enemy from './Enemy.js';
import PowerUp from './PowerUp.js';

export default class GameScene extends Phaser.Scene {
    constructor() {
        super({ key: 'GameScene' });
    }

    init(data) {
        this.mode = data.mode;
        this.killsNeeded = this.mode === 'hard' ? 20 : 2; // kills necesarias para ganar (2 en ez para ver cómo va quedando)
        this.killsLeft = this.killsNeeded; // contador descendente que se muestra en pantalla
        this.playerLives = 4; // vidas = número de lobos que pueden llegar al suelo antes de perder
    }

    preload() {
        this.load.image('background', 'assets/background.png');
        this.load.spritesheet('player', 'assets/pooyan.png',
            { frameWidth: 24, frameHeight: 32 });
        this.load.spritesheet('player_dead', 'assets/dead.png',
            { frameWidth: 24, frameHeight: 24 });
        this.load.spritesheet('wolf', 'assets/wolf.png',
            { frameWidth: 18, frameHeight: 28 });
        this.load.image('arrow', 'assets/arrow.png');
        this.load.image('meat', 'assets/meat.png');
        this.load.image('pig', 'assets/pig.png');
        this.load.image('icon', 'assets/icon.png');
        this.load.audio('balloon', 'assets/balloon.mp3');
        this.load.audio('shot', 'assets/shot.mp3');
        this.load.audio('win', 'assets/win.mp3');
        this.load.audio('loose', 'assets/loose.mp3');
    }

    create() {
        this.add.image(0, 0, 'background').setOrigin(0, 0);

        if (!this.anims.exists('idle_arrow')) {
            // Player
            this.anims.create({
                key: 'idle_arrow', frames: this.anims.generateFrameNumbers
                    ('player', { start: 0, end: 0 }), frameRate: 5, repeat: 0
            });
            this.anims.create({
                key: 'idle_meat', frames: this.anims.generateFrameNumbers
                    ('player', { start: 2, end: 2 }), frameRate: 20, repeat: -1
            });
            this.anims.create({
                key: 'shoot', frames: this.anims.generateFrameNumbers
                    ('player', { start: 1, end: 1 }), frameRate: 15, repeat: 0 // repeat:0 → se queda en frame 1 durante la recarga
            });
            this.anims.create({
                key: 'empty', frames: this.anims.generateFrameNumbers
                    ('player', { start: 3, end: 3 }), frameRate: 5, repeat: 0 // frame 3: cabina vacía cuando pooyan muere
            });
            this.anims.create({
                key: 'player_die', frames: this.anims.generateFrameNumbers
                    ('player_dead', { start: 0, end: 1 }), frameRate: 5, repeat: 0 // spritesheet dead.png, no pooyan.png
            });
            // Lobo
            this.anims.create({
                key: 'wolf_run', frames: this.anims.generateFrameNumbers
                    ('wolf', { start: 7, end: 10 }), frameRate: 5, repeat: -1
            });
            this.anims.create({
                key: 'wolf_run_balloon', frames: this.anims.generateFrameNumbers
                    ('wolf', { start: 5, end: 6 }), frameRate: 5, repeat: -1 // corre por el suelo con globo intacto
            });
            this.anims.create({
                key: 'wolf_die', frames: this.anims.generateFrameNumbers
                    ('wolf', { start: 11, end: 11 }), frameRate: 5, repeat: 0
            });
            this.anims.create({
                key: 'wolf_fall', frames: this.anims.generateFrameNumbers
                    ('wolf', { start: 12, end: 12 }), frameRate: 3, repeat: 0 // cae sin globo (le han explotado)
            });
            this.anims.create({
                key: 'wolf_down', frames: this.anims.generateFrameNumbers
                    ('wolf', { start: 13, end: 13 }), frameRate: 10, repeat: 0 // muerte en el suelo tras caer sin globo
            });
            this.anims.create({
                key: 'wolf_blow', frames: this.anims.generateFrameNumbers
                    ('wolf', { start: 0, end: 4 }), frameRate: 5, repeat: 0
            });
            this.anims.create({
                key: 'wolf_idle', frames: this.anims.generateFrameNumbers
                    ('wolf', { start: 3, end: 4 }), frameRate: 3, repeat: -1 // flotando con globo hinchado mientras cae
            });
        }

        this.player = new Player(this, 213, 120);

        // icon.png + texto con el contador
        this.add.image(8, 8, 'icon').setOrigin(0, 0).setDepth(10);
        this.killsText = this.add.text(20, 8, 'x' + this.killsLeft, {
            fontFamily: 'arcade',
            fontSize: '8px',
            color: '#ffffff'
        }).setOrigin(0, 0).setDepth(10);

        // vidas: array de imágenes pig.png - se crean de derecha a izquierda
        this.lifeIcons = [];
        for (let i = 0; i < this.playerLives; i++) {
            let pig = this.add.image(
                this.sys.game.config.width - 8 - (i * 10), 8, 'pig' // i*10: cada pig 10px más a la izquierda
            ).setOrigin(1, 0).setDepth(10);
            this.lifeIcons.push(pig);
        }

        this.projectiles = this.physics.add.group();
        this.enemies = this.physics.add.group();

        // overlap flecha/carne vs lobos: un solo detector para todos los proyectiles y todos los lobos
        this.physics.add.overlap(this.projectiles, this.enemies, (projectile, wolf) => {
            if (wolf.state !== 'falling') return; // solo vulnerable mientras cae con globo
            if (projectile.processed) return; // ya fue procesada este frame, evita doble detección

            if (projectile.isMeat) {
                wolf.popGlobe(); // carne: mata siempre, NO se destruye (sigue su trayectoria)
                return;
            }

            // flecha: mitad superior (y < wolf.y = donde está el globo) o inferior
            let hitTop = projectile.y < wolf.y;
            if (hitTop) {
                wolf.popGlobe();
                projectile.destroy(); // la flecha se destruye al golpear el globo
            } else {
                projectile.processed = true; // marcar ANTES de cambiar velocidades para evitar reentrada
                wolf.hitBottom(projectile); // la flecha sale disparada hacia abajo
            }
        });

        // spawn de lobos en bucle, sin límite: la victoria viene de killsLeft, no de cuántos spawneas
        this.time.addEvent({
            delay: 2000,
            callback: this.spawnEnemy,
            callbackScope: this,
            loop: true
        });

        this.spawnPowerUp();

        this.gameEnded = false; // guarda para que onVictory/onDefeat no se disparen 60 veces por segundo desde update
    }

    update() {
        this.player.update();
        this.enemies.getChildren().forEach(e => e.update());

        // limpiar proyectiles fuera de pantalla
        this.projectiles.getChildren().forEach(p => {
            if (p.x < -20 || p.x > this.sys.game.config.width + 20 ||
                p.y > this.sys.game.config.height + 20) {
                p.destroy();
            }
        });
    }

    loseLife() {
        this.playerLives--;
        let icon = this.lifeIcons.pop(); // pop: saca y devuelve el último elemento del array
        if (icon) icon.destroy();

        if (this.playerLives <= 0) {
            this.onDefeat();
        }
    }

    killEnemy() {
        this.killsLeft--;
        this.killsText.setText('x' + this.killsLeft);

        if (this.killsLeft <= 0) {
            this.onVictory();
        }
    }

    onVictory() {
        if (this.gameEnded) return;
        this.gameEnded = true;
        this.sound.play('win');
        this.time.delayedCall(2000, () => {
            this.scene.start('GameOverScene', { win: true });
        });
    }

    onDefeat() {
        if (this.gameEnded) return;
        this.gameEnded = true;
        this.sound.play('loose');

        this.player.active = false; // bloquea el update() del player: sin input, sin disparo
        this.player.body.setAllowGravity(true); // el jugador cae físicamente al morir
        this.player.setTexture('player_dead'); // swap de textura a dead.png
        this.player.play('player_die');
        this.player.setVelocityY(60); // empuje inicial hacia abajo para que caiga

        this.time.delayedCall(2000, () => {
            this.scene.start('GameOverScene', { win: false });
        });
    }

    spawnEnemy() {
        let wolf = new Enemy(this, 0, 20); // aparece por la izquierda arriba del árbol
        this.enemies.add(wolf);
        // Reaplicamos velocidad y gravedad dsps del add:
        wolf.setVelocityX(50);
        wolf.body.setAllowGravity(false);
    }

    spawnPowerUp() {
        let y = Phaser.Math.Between(this.player.minY, this.player.maxY); // posición aleatoria en la zona del jugador
        this.powerUp = new PowerUp(this, 213, y); // aparece en la misma X que el jugador

        this.physics.add.overlap(this.player, this.powerUp, () => {
            this.player.collectPowerUp(); // activa hasMeat en el player
            this.powerUp.destroy();
            this.powerUp = null; // null para evitar referencias a objeto destruido

            this.time.delayedCall(10000, () => { // reaparece a los 10s
                this.spawnPowerUp();
            });
        });
    }
}