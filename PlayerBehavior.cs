using UnityEngine;
using System.Collections;

public enum PlayerEstaNo{
	SOLO,
	AR
}

public class PlayerBehavior : MonoBehaviour {

	//Enum
	private PlayerEstaNo playerEstaNo;


	//Movimentacao
	public float velCorrer;
	private float velAtual;
	private float velocidade;
	public float velAndar;
	public float aceleracao;

	//Salto
	public float saltoMax;
	public float saltoMin;

	//relacionado a verificaçao de solo abaixo dos pes do player
	public Transform contatoSolo;
	public float raioContatoSolo;
	public LayerMask tipoContato;
	private bool emSolo;



	//relacionado a verificaçao de paredes impedindo a movimentaçao
	public Transform contatoParedeFrente;
	public Transform contatoParedeAtras;
	public float raioContatoParede;
	private bool paredeNaFrente;
	private bool paredeAtras;

	//Componentes
	private Rigidbody2D playerRigidBody;

	//teclas
	public KeyCode keyEsquerda;
	public KeyCode keyDireita;
	public KeyCode keyPular;
	public KeyCode keyCorrer;


	void Start()
	{
		//
		playerRigidBody = GetComponent<Rigidbody2D>();
		playerEstaNo = PlayerEstaNo.AR;
	}

	void FixedUpdate()
	{
		//checar se existe solo abaixo dos pes do player
		emSolo = Physics2D.OverlapCircle(contatoSolo.position, raioContatoSolo, tipoContato);

		//checar se existe parede na frente impedindo o movimento do player para frente
		paredeNaFrente = Physics2D.Raycast(transform.position, Vector2.right, raioContatoParede);

	

		//checar se existe parede atras impedidno o movimento do player para tras
		paredeAtras = Physics2D.Raycast(transform.position, -Vector2.right, raioContatoParede);

		if(Physics2D.Raycast(contatoSolo.position, -Vector2.up, raioContatoSolo, LayerMask.GetMask("Inimigo")))
		{
			Salto();
		}


	}

	void Update () 
	{



		// o player esta no ar ou no solo?
		switch(emSolo)
			{
			case false:
				{
					playerEstaNo = PlayerEstaNo.AR;
				}
				break;
			case true:
				{
					playerEstaNo = PlayerEstaNo.SOLO;
				}
				break;
			}


	
		//saltar
		if(Input.GetKeyDown(keyPular) && emSolo)
		{
			Debug.Log("salto");
			Salto();
		}

		//andar para frente
		if(Input.GetKey(keyDireita))
		{
			//Se eu for para direita, e o personagem estiver na inercia indo para esquerda, interrompo a inercia
			//setando a velocidade como 0, para ele começar a ir para a direita
			if(velAtual <0 && !(Input.GetKey(keyEsquerda))){
				//no chao
				if(emSolo){
					velAtual = 0;
					playerRigidBody.velocity = new Vector2(-playerRigidBody.velocity.x,playerRigidBody.velocity.y);
				}
				//mobilidade no ar
				else{
					velAtual = -velAtual;
					playerRigidBody.velocity = new Vector2(-playerRigidBody.velocity.x*(velAtual/2),playerRigidBody.velocity.y);
				}
			}


			//Correr
			Correr(1);
			//movimentar player para a direita
			MovimentarDireita();



		}

	

		//andar para tras
		if(Input.GetKey(keyEsquerda))
		{


			if(velAtual >0 && !(Input.GetKey(keyDireita))){
				//no chao
				if(emSolo){
					velAtual = 0;
					playerRigidBody.velocity = new Vector2(0,playerRigidBody.velocity.y);
				}
				//mobilidade no ar
				else{
					velAtual = -velAtual;
					playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x*(velAtual/2),playerRigidBody.velocity.y);
				}
			}
				
				//Correr
				Correr(-1);
				//movimentar player para esquerda
				MovimentarEsquerda();

		}

		if(!(Input.GetKey(keyEsquerda)) && !(Input.GetKey(keyDireita)) && playerEstaNo == PlayerEstaNo.SOLO)
		{
			velAtual = playerRigidBody.velocity.x;
			velocidade=0;
		}


	}

	void Correr(int sentido)
	{
	
		if(Input.GetKey(keyCorrer))
		{
			if(emSolo)
			{
				//velocidade do player correndo
				velocidade = velCorrer * sentido;
			}
		}
		//tecla de correr nao apertada
		else
		{

				velocidade = velAndar * sentido;
		
		}

	
	}

	//saltar
	void Salto()
	{
		playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x,saltoMin);
	}




	//andar para frente
	void MovimentarDireita()
	{
		//se nao tiver parede na frente do player
		if(!paredeNaFrente)
		{
			//se a velocidade atual do player for menor que a velocidade maxima que ele vai atingir andando ou correndo
			//velocidade escala conforme a aceleraçao
			if(velAtual<=velocidade )
			{
				//if(emSolo)
					velAtual += aceleracao*Time.deltaTime;
				/*else
					velAtual += (aceleracao/2)*Time.deltaTime;*/
			}

			if(velAtual>velocidade)
			{
				//velAtual = velocidade + 0.1f;
				velAtual -= (aceleracao/1.5f)*Time.deltaTime;

			}
			playerRigidBody.velocity = new Vector2(velAtual,playerRigidBody.velocity.y);
		}
	}

	//antar para tras
	void MovimentarEsquerda()
	{
		if(!paredeAtras)
		{
			if(velAtual>=velocidade )
			{
				velAtual -= aceleracao*Time.deltaTime;
			}

			if(velAtual<velocidade)
			{
				//velAtual = velocidade + 0.1f;
				velAtual += (aceleracao/1.5f)*Time.deltaTime;

			}

			playerRigidBody.velocity = new Vector2(velAtual,playerRigidBody.velocity.y);
		}
	}


}


