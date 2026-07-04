export default class Disc extends Phaser.Physics.Arcade.Sprite {
	constructor(scene, x, y, direction, owner, speed = 200) {
		super(scene, x, y, 'disc');

		scene.add.existing(this);
		scene.physics.add.existing(this);

		this.owner = owner;
		this.speed = speed;
		this.state = 'thrown'; // 'thrown' (hace daño) | 'returning' (vuelve, no hace daño)

		this.setVelocity(direction.x * this.speed, direction.y * this.speed);
		this.rotation = Math.atan2(direction.y, direction.x);
	}

	bounce() {
		if (this.state !== 'thrown') return; // ya estaba volviendo, ignoramos doble choque
		this.state = 'returning';
		this.setTexture('discOff');
	}

	forceReturn() {
		if (this.state !== 'thrown') return;
		this.state = 'returning';
		this.setTexture('discOff');
	}

	update() {
		if (this.state !== 'returning') return;

		let distance = Phaser.Math.Distance.Between(this.x, this.y, this.owner.x, this.owner.y);

		if (distance < 10) {
			this.owner.disc = null;
			this.destroy();
			return;
		}

		this.scene.physics.moveToObject(this, this.owner, this.speed);
		this.rotation = Phaser.Math.Angle.Between(this.x, this.y, this.owner.x, this.owner.y);
	}
}

/* EXPLICACIONES

ESTRUCTURA GENERAL
-------------------
Disc representa al disco mortal, y es la pieza más reutilizada de todo el proyecto: la MISMA
clase sirve tanto para el disco que lanza el jugador como para el que lanza cualquier enemigo.
Esto es posible porque la clase no sabe ni le importa quién es su dueño concreto: solo guarda
una referencia genérica (this.owner) y confía en que ese owner tenga propiedades x/y (posición)
y una propiedad disc (para poder "liberarse" cuando lo recogen). Tanto Player como Enemy cumplen
ese "contrato" sin que haya que escribir herencia ni interfaces explícitas (JS es de tipado
duck-typing: si camina como un pato y grazna como un pato, sirve).

constructor(scene, x, y, direction, owner, speed = 200)
---------------------------------------------------------
- scene, x, y: estándar de cualquier Phaser.Physics.Arcade.Sprite, posición inicial de
  aparición (las coordenadas del personaje que lo lanza en el momento del lanzamiento).
- direction: objeto {x, y} ya normalizado (longitud 1) que indica hacia dónde vuela. Lo calcula
  quien construye el disco (Player.getThrowDirection() o el cálculo equivalente en Enemy), no
  el propio Disc.
- owner: referencia al objeto (Player o Enemy) que lo lanzó. Se usa para: (a) saber a quién
  perseguir cuando vuelve, y (b) poder poner owner.disc = null cuando se recoge, liberando al
  dueño para que pueda lanzar otro.
- speed = 200: velocidad por defecto. Tiene valor por defecto porque el jugador siempre lanza a
  la misma velocidad fija, pero los enemigos lanzan más rápido cuanto más avanzada esté la
  oleada (se les pasa this.speed + 100 desde Enemy.tryThrowDisc(), sobreescribiendo el default).

super(scene, x, y, 'disc') inicializa el Sprite con la textura 'disc' (el rectángulo cargado
desde disc.png). scene.add.existing(this) lo registra en la lista de objetos visibles de la
escena; scene.physics.add.existing(this) le da un body de física arcade (sin esto, setVelocity
no existiría sobre este objeto). Ambas llamadas son obligatorias y no las da gratis extender
Phaser.Physics.Arcade.Sprite: extender la clase te da los MÉTODOS, pero seguís teniendo que
registrar la instancia a mano en la escena y en el mundo físico.

this.state = 'thrown': variable de estado interna, inicializada explícitamente aquí. Es crítico
que tenga un valor desde el primer instante porque bounce() y update() comparan contra strings
concretos ('thrown'/'returning'); si se quedara como undefined, cualquier comparación
!== 'thrown' sería true por defecto y la lógica de rebote nunca se dispararía (bug real que
nos costó detectar durante el desarrollo).

setVelocity(direction.x * speed, direction.y * speed): como direction ya viene normalizado
(magnitud 1), multiplicarlo por speed da un vector cuya magnitud es exactamente speed, en la
dirección correcta. this.rotation = Math.atan2(direction.y, direction.x) orienta visualmente el
sprite del disco (que es un rectángulo alargado) para que apunte en la dirección de vuelo;
atan2 calcula el ángulo de un vector (y, x) sin que tengamos que hacer la trigonometría a mano
ni preocuparnos de los casos límite (x=0, etc.) que Math.atan normal no resuelve bien.

bounce()
--------
Se llama desde el collider entre el disco y el grupo de barreras (registrado en
Player.tryThrowDisc() / Enemy.tryThrowDisc() en el momento de crear el disco). La guarda
if (this.state !== 'thrown') return evita procesar un "rebote" si el disco ya estaba volviendo
(por ejemplo, si por la física quedara solapado un instante con la barrera mientras vuelve, no
queremos que se reinicie nada). Al rebotar: cambia el estado a 'returning' (a partir de ahora
no hará daño, ver overlaps en Player/Enemy que comprueban el estado) y cambia la textura visual
a 'discOff' (el círculo, representando que "ha perdido la fuerza").

Importante: bounce() NO toca la velocidad del disco. Quien se encarga de moverlo de vuelta es
update(), llamado cada frame externamente.

forceReturn()
-------------
Casi idéntico a bounce(), pero se llama cuando el JUGADOR pulsa espacio una segunda vez antes
de que el disco choque con nada (forzando la vuelta anticipada). Tiene su propio método (en vez
de simplemente llamar a bounce() desde fuera) porque conceptualmente son dos disparadores
distintos (colisión física vs. input del jugador), aunque hagan lo mismo internamente. Si en el
futuro quisiéramos, por ejemplo, no cambiar la textura en el forceReturn pero sí en el bounce,
ya tendríamos los dos sitios separados para diferenciarlos sin tocar la lógica del otro.

update()
--------
Se llama manualmente cada frame, pero SOLO desde dentro del update() del dueño (Player.update()
hace this.disc.update() si this.disc existe; Enemy.update() hace lo mismo). Phaser NO llama
automáticamente al update() de objetos custom como este: solo llama al update()/preUpdate() de
las Scenes. Por eso cualquier objeto con lógica por-frame necesita que "alguien" en la cadena
de update()s decida llamarlo explícitamente.

Si el estado no es 'returning', no hace nada (el disco en estado 'thrown' simplemente vuela en
línea recta gracias a la velocity que ya tiene puesta desde el constructor; no necesita
recalcularse cada frame).

Si es 'returning':
1. Calcula la distancia actual hasta el dueño (Phaser.Math.Distance.Between, equivalente a
   Pitágoras sin tener que escribirlo a mano).
2. Si la distancia es menor que 10px, lo consideramos "recogido": liberamos al dueño
   (owner.disc = null, para que pueda volver a lanzar) y destruimos el disco (this.destroy()
   lo quita de la escena, del mundo físico y libera su memoria). El umbral de 10px (en vez de
   comparar igualdad exacta) es necesario porque es prácticamente imposible que dos posiciones
   en coma flotante coincidan pixel a pixel en un juego en movimiento continuo.
3. Si todavía no ha llegado, this.scene.physics.moveToObject(this, this.owner, this.speed)
   recalcula la velocidad del disco apuntando hacia la posición ACTUAL del dueño (no hacia
   donde estaba cuando rebotó), lo que produce el efecto de "persecución" (homing): si el
   jugador se mueve mientras el disco vuelve, el disco corrige su trayectoria en tiempo real
   porque este cálculo se repite cada frame. this.rotation se actualiza igual, para que el
   dibujo del disco siga apuntando visualmente hacia donde va.

POR QUÉ NO HAY DAÑO DENTRO DE ESTA CLASE
-------------------------------------------
A propósito, Disc no sabe nada de "vidas" ni de a quién puede dañar. Esa lógica vive en los
overlaps que registran Player y Enemy al crear el disco (this.scene.physics.add.overlap(...)),
comprobando ellos mismos this.disc.state === 'thrown' antes de restar una vida. Esto mantiene a
Disc como una clase "tonta" y genérica (solo sabe volar, rebotar y volver), y deja la decisión
de "esto me hace daño a mí" en manos de quien recibe el golpe — más limpio que meter referencias
cruzadas a Player/Enemy dentro de Disc.

*/
