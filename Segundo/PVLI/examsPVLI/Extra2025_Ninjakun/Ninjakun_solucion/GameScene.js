import Platform from './Platform.js';
import Player from './Player.js';
import Enemy from './Enemy.js';
import Shuriken from './Shuriken.js';

export default class GameScene extends Phaser.Scene {
    constructor() {
        super({ key: 'GameScene' });
    }

    create() {
        this.add.image(0, 0, 'bg').setOrigin(0, 0);

        this.physics.world.setBounds(0, 0, 256, 480);
        this.cameras.main.setBounds(0, 0, 256, 480);

        let infoPlatforms = [
            { y: 48, left: 80, right: 192 },
            { y: 96, left: 48, right: 176 },
            { y: 144, left: 96, right: 208 },
            { y: 192, left: 24, right: 176 },
            { y: 240, left: 72, right: 224 },
            { y: 288, left: 40, right: 232 },
            { y: 336, left: 56, right: 184 },
            { y: 384, left: 80, right: 208 },
        ];

        this.platforms = this.physics.add.staticGroup();
        infoPlatforms.forEach(p => {
            let plat = new Platform(this, p.y, p.left, p.right);
            this.platforms.add(plat);
        });
        // suelo inicial
        let ground = new Platform(this, 434, 0, 256); // ancho completo del mapa
        this.platforms.add(ground);

        // jugador nace en el suelo
        this.player = new Player(this, 128, 410);
        this.physics.add.collider(this.player, this.platforms);

        // cámara sigue al jugador
        this.cameras.main.startFollow(this.player, true, 0.1, 0.1);

        // enemigos: uno por plataforma
        this.enemies = this.physics.add.group();
        infoPlatforms.forEach(p => {
            let centerX = (p.left + p.right) / 2;
            let enemy = new Enemy(this, centerX, p.y - 10, p.left, p.right);
            this.enemies.add(enemy);
            enemy.body.allowGravity = false;
            enemy.setVelocityX(enemy.facing === 'left' ? -enemy.speed : enemy.speed);
        });

        this.shurikens = this.physics.add.group();

        // overlaps
        this.physics.add.overlap(this.shurikens, this.enemies, (shuriken, enemy) => {
            if (shuriken.owner === 'player' && shuriken.body.enable) {
                shuriken.pop();
                enemy.die();
                this.enemies.remove(enemy); // lo sacamos del grupo YA (antes de que destroy() lo haga solo)
                this.addScore(100);
                this.checkVictory();
            }
        });

        this.physics.add.overlap(this.shurikens, this.player, (obj1, obj2) => {
            // determinamos cuál de los dos es el shuriken, sin asumir el orden
            let shuriken = obj1.owner !== undefined ? obj1 : obj2;

            if (shuriken.owner === 'enemy' && shuriken.body.enable) {
                shuriken.pop();
                this.onPlayerDeath();
            }
        });

        this.physics.add.overlap(this.shurikens, this.shurikens, (s1, s2) => {
            if (s1 !== s2 && s1.body.enable && s2.body.enable) {
                s1.pop();
                s2.pop();
            }
        });

        this.physics.add.overlap(this.player, this.enemies, (player, enemy) => {
            if (!enemy.stunned) {
                enemy.getStunned();
            }
        });

        // boost cada 7 segundos
        this.time.addEvent({
            delay: 7000,
            callback: () => this.spawnBoost(),
            loop: true
        });

        this.score = 0;
        this.scoreText = this.add.text(128, 16, '0', {
            fontFamily: 'upheat', fontSize: '16px', color: '#ffffff'
        }).setOrigin(0.5).setDepth(10).setScrollFactor(0);

        this.gameEnded = false;
    }

    update() {
        if (this.gameEnded) return;
        this.player.update();
        this.enemies.getChildren().forEach(e => e.update());
    }

    spawnShuriken(x, y, direction, owner) {
        let shuriken = new Shuriken(this, x, y, direction, owner);
        this.shurikens.add(shuriken);
        shuriken.body.setAllowGravity(false); // reaplicar tras el add
        shuriken.setVelocityX(direction === 'left' ? -shuriken.speed : shuriken.speed); // reaplicar tras el add
    }

    spawnBoost() {
        // aparece en la parte superior del mapa
        let x = Phaser.Math.Between(30, 226);
        let boost = this.physics.add.sprite(x, 10, 'boost');
        boost.body.setAllowGravity(false);
        boost.setVelocityY(40);

        // pequeña animación de tamaño
        this.tweens.add({
            targets: boost,
            scale: 1.4,
            duration: 400,
            yoyo: true,
            repeat: -1
        });

        this.physics.add.overlap(this.player, boost, () => {
            this.addScore(300);
            this.sound.play('boost_sound');
            boost.destroy();
        });

        // si llega abajo del todo sin ser recogido, se destruye
        this.time.delayedCall(15000, () => {
            if (boost.active) boost.destroy();
        });
    }

    addScore(points) {
        this.score += points;
        this.scoreText.setText(this.score);
    }

    checkVictory() {
        if (this.enemies.countActive() === 0) {
            this.addScore(600);
            this.endGame(true);
        }
    }

    onPlayerDeath() {
        if (this.gameEnded) return;
        this.gameEnded = true;

        this.player.play('player_die');
        this.player.setVelocity(0, 0);
        this.sound.play('player_die');

        this.time.delayedCall(2000, () => {
            this.endGame(false);
        });
    }

    endGame(win) {
        let hiScore = parseInt(localStorage.getItem('hiScore')) || 0;
        let newRecord = this.score > hiScore;
        if (newRecord) {
            localStorage.setItem('hiScore', this.score);
        }

        this.scene.start('GameOverScene', {
            win: win,
            score: this.score,
            hiScore: newRecord ? this.score : hiScore,
            newRecord: newRecord
        });
    }
}