using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class global_var
{
    public static bool isStart = false;
    public static bool isPause = false;
}
public class Manager : MonoBehaviour {

    public Text gameoverText;
    public Text scoreText;
    public Text highestText;
    public GameObject StartButtom;
    public GameObject ingameMenu;
    public GameObject pauseMenu;
    public GameObject[] blocks;
	public Transform cube;
	public Transform leftWall;
	public Transform rightWall;
	public int maxBlockSize = 4;
	public int _fieldWidth = 10;
	public int _fieldHeight = 16;
	public float blockNormalFallSpeed = 2f;
	public float blockDropSpeed = 30f;
	public Texture2D cubeTexture;
	
	private int fieldWidth;
	private int fieldHeight;
	private bool[,] fields;
	private int[] cubeYposition;
	private Transform[] cubeTransforms;
	private int clearTimes;
	private float addSpeed = 1.0f;
	private int TimeToAddSpeed = 10;
	
	private int Score = 0;
	private int Highest = 0;
	private int blockRandom;
	private GameObject nextBlock;
	private Block nextB;
	private int nextSize;
	private string[] nextblock;
	
	public static Manager manager;

	// Use this for initialization
	void Start () {
	
		if (manager == null){
			manager = this;
		}
        Time.timeScale = 0;
        Score = 0;
        scoreText.text = "Score: " + Score.ToString();

        if (PlayerPrefs.HasKey("Highest")){
			Highest = PlayerPrefs.GetInt("Highest");
            highestText.text = "Highest: " + Highest.ToString();

        }
		else{
			PlayerPrefs.SetInt("Highest", 0);
            highestText.text = "Highest: " + Highest.ToString();
        }
        gameoverText.text = "";
        blockRandom = Random.Range(0, blocks.Length);
		
		fieldWidth = _fieldWidth + maxBlockSize * 2;
		fieldHeight = _fieldHeight + maxBlockSize;
		fields = new bool[fieldWidth, fieldHeight];
		cubeYposition = new int[fieldHeight * fieldWidth];
		cubeTransforms = new Transform[fieldHeight * fieldWidth];
		
		for (int i = 0;i < fieldHeight;i++){
			
			for (int j =0 ;j < maxBlockSize;j++){
				
				fields[j, i] = true;
				fields[fieldWidth -1 - j, i] = true;
				
			}
			
		}
		
		for (int i=0;i<fieldWidth;i++){
			fields[i, 0] = true;
		}
		
		CreateBlock(blockRandom);
	}
	
	void CreateBlock(int random){
		Instantiate(blocks[random]);
		blockRandom = Random.Range(0, blocks.Length);
		nextBlock = blocks[blockRandom];
		nextB = (Block)nextBlock.GetComponent("Block");
		nextSize = nextB.block.GetLength(0);
		nextblock = new string[nextSize];
		nextblock = nextB.block;
	}
	
	public int GetFieldWidth(){
		return fieldWidth;
	}
	
	public int GetFieldHeight(){
		return fieldHeight;
	}
	
	public int GetBlockRandom(){
		return blockRandom;
	}
	
	public bool CheckBlock(bool [,] blockMatrix, int xPos, int yPos){

		var size = blockMatrix.GetLength(0);
		for (int y = 0;y < size;y++){
			for (int x = 0;x < size;x++){
				if (blockMatrix[y, x] && fields[xPos + x, yPos - y]){
					return true;
				}
			}
		}
		
		return false;
	}
	
	public void SetBlock(bool[,] blockMatrix, int xPos, int yPos){
		
		int size = blockMatrix.GetLength(0);
		for (int y = 0;y < size;y++){
			for (int x = 0;x < size;x++){
				if (blockMatrix[y, x]){
					Instantiate(cube, new Vector3(xPos + x, yPos - y, 0), Quaternion.identity);
					fields[xPos + x, yPos - y] = true;
				}
			}
		}
		StartCoroutine(CheckRows(yPos - size, size));
		
	}
	
	IEnumerator CheckRows(int yStart, int size){
		yield return null;
		if (yStart < 1)yStart = 1;
		int count = 1;
		for (int y = yStart;y < yStart + size;y++){
			int x;
			for (x = maxBlockSize;x < fieldWidth - maxBlockSize;x++){
				if (!fields[x, y]){
					break;
				}
			}
			if (x == fieldWidth - maxBlockSize){
				yield return StartCoroutine(SetRows(y));
				Score += 10 * count;
				y--;
				count++;
                scoreText.text = "Score: " + Score.ToString();
            }
		}
		CreateBlock(blockRandom);
	}
	
	IEnumerator SetRows(int yStart){
		for (int y = yStart;y < fieldHeight - 1;y++){
			for (int x = maxBlockSize;x < fieldWidth - maxBlockSize;x++){
				fields[x, y] = fields[x, y + 1];
			}
		}
		
		for (int x = maxBlockSize;x < fieldWidth - maxBlockSize;x++){
			fields[x, fieldHeight - 1] = false;
		}
		
		var cubes = GameObject.FindGameObjectsWithTag("Cube");
		int cubeToMove = 0;
		for (int i = 0;i < cubes.Length;i++){
			GameObject cube = cubes[i];
			if (cube.transform.position.y > yStart){
				cubeYposition[cubeToMove] = (int)(cube.transform.position.y);
				cubeTransforms[cubeToMove++] = cube.transform;
			}
			else if (cube.transform.position.y == yStart){
				Destroy(cube);
			}
		}
		
		float t = 0;
		while (t <= 1f){
			t += Time.deltaTime * 5f;
			for(int i = 0;i < cubeToMove;i++){
				cubeTransforms[i].position = new Vector3(cubeTransforms[i].position.x, Mathf.Lerp(cubeYposition[i], cubeYposition[i] - 1, t),
					cubeTransforms[i].position.z);
			}
		    yield return null;
		}
		
		if (++clearTimes == TimeToAddSpeed){
			blockNormalFallSpeed += addSpeed;
			clearTimes = 0;
		}
		
	}
	public void GameStart()
    {
        global_var.isStart = true;
        Time.timeScale = 1f;
        StartButtom.SetActive(false);
        pauseMenu.SetActive(true);

    }
	public void GameOver(){
        global_var.isStart = false;
        if (Score > PlayerPrefs.GetInt("Highest")){
			PlayerPrefs.SetInt("Highest", Score);
            highestText.text = "Highest: " + Highest.ToString();
        }
        pauseMenu.SetActive(false);
        gameoverText.text = "Game Over!!!";
        ingameMenu.SetActive(true);
    }
	
	void OnGUI(){	
		for (int y = 0;y < nextSize;y++){
			for (int x = 0;x < nextSize;x++){
				if (nextblock[y][x] == '1'){
					GUI.Button(new Rect(180 + 30 * x, 100 + 30 * y, 30, 30), cubeTexture);
				}
			}
		}
	}
	
}
