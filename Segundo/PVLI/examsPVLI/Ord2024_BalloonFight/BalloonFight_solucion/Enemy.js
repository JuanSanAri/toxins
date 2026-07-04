export default class Enemy {
	constructor(scene, x, y) {
		this.scene = scene;

		this.doll = scene.physics.add.sprite(x, y, 'enemy');

		this.balloon = scene.physics.add.sprite(x, y - 12, 'balloon');
		this.balloon.body.setAllowGravity(false);
		this.balloon.body.moves = false;
		this.balloon.setTint(0x00ff00); // verde para diferenciarlo visualmente del globo del jugador

		this.hasBalloon = true;
		this.speed = Phaser.Math.Between(20, 30); // velocidad aleatoria decidida UNA vez al crearse, no cambia
		this.invulnerableAfterPop = false; // flag: 1s de invulnerabilidad tras perder el globo (ver popBalloon)

		this.doll.play('enemy_fly');
		this.balloon.play('balloon_normal');
	}

	update() {
		// el globo sigue al muñeco
		this.balloon.x = this.doll.x;
		this.balloon.y = this.doll.y - 12;

		// flip horizontal según el sentido de movimiento actual
		if (this.doll.body.velocity.x !== 0) {
			let lookLeft = this.doll.body.velocity.x > 0;
			this.doll.setFlipX(lookLeft);
			this.balloon.setFlipX(lookLeft);
		}

		if (!this.hasBalloon) return; // sin globo: cae libre por gravedad, no persigue al jugador

		// recalculamos la dirección hacia el jugador cada frame
		let player = this.scene.player.doll;
		let dx = player.x - this.doll.x;
		let dy = player.y - this.doll.y;
		let length = Math.sqrt(dx * dx + dy * dy) || 1; // || 1 evita división por cero si coinciden en el mismo píxel

		// vector normalizado * speed: misma velocidad en cualquier dirección
		this.doll.setVelocityX((dx / length) * this.speed);
		this.doll.setVelocityY((dy / length) * this.speed);
	}

	popBalloon() {
		if (!this.hasBalloon) return;

		this.doll.setImmovable(true); // el jugador rebota en él tipo Mario, el enemigo no sale disparado
		this.hasBalloon = false;
		this.balloon.play('balloon_pop');
		this.scene.time.delayedCall(300, () => { // 300ms para ver la anim antes de ocultar
			this.balloon.setVisible(false);
			this.balloon.body.enable = false; // desactiva colisiones del globo
		});
		this.doll.play('enemy_fall'); // anim de caída libre
		this.doll.setVelocity(0, 0); // cancela la velocidad de persecución, cae libre desde cero

		this.invulnerableAfterPop = true; // 1s de gracia para que el jugador no lo mate instantáneamente al caer
		this.scene.time.delayedCall(1000, () => {
			this.invulnerableAfterPop = false;
		});

		// si nadie lo mata en 10s, recupera el globo y vuelve a perseguir
		this.regenTimer = this.scene.time.delayedCall(10000, () => {
			this.regenerateBalloon();
		});
	}

	regenerateBalloon() {
		this.hasBalloon = true;
		this.balloon.setVisible(true);
		this.balloon.body.enable = true; // reactiva colisiones del globo
		this.balloon.play('balloon_normal');
		this.doll.setImmovable(false); // vuelve a ser empujable por la física
		this.doll.play('enemy_fly');
	}

	die() {
		if (this.regenTimer) this.regenTimer.remove(); // cancela el timer de regeneración si sigue activo
		this.doll.play('enemy_dead');
		this.scene.time.delayedCall(500, () => { // 500ms para ver el frame de muerte antes de destruir
			this.balloon.destroy();
			this.doll.destroy();
		});
	}
}