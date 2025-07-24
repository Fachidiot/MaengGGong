using UnityEngine;
using UnityEditor;
using System.Collections;

public class GenerateCubeMap : ScriptableWizard
{
    [Tooltip("큐브맵을 렌더링할 기준 위치입니다.")]
	public Transform renderPosition;
    [Tooltip("렌더링 결과를 저장할 큐브맵 에셋입니다.")]
    public Cubemap cubemap;
	// Use this for initialization 
	void OnWizardUpdate () {


		helpString = "큐브맵을 렌더링할 위치(Transform)와 결과물을 저장할 큐브맵(Cubemap) 에셋을 선택하세요.";
		if (helpString != null && cubemap != null) 
		{
			isValid = true;
		}
		else 
		{
			isValid = false;
		}
	}

	void OnWizardCreate()
	{
		//렌더링을 위한 임시 카메라 생성 
		GameObject go = new GameObject ("CubeCam", typeof(Camera));

		//카메라를 렌더링 위치에 놓는다. 
		go.transform.position = renderPosition.position;
		go.transform.rotation = Quaternion.identity;
        
        Camera cam = go.GetComponent<Camera>();

        //큐브맵 렌더링 
        // go.camera.RenderToCubemap (cubemap);
        cam.RenderToCubemap(cubemap);

		//임시카메라 제거 
        DestroyImmediate (go);
	}
	
	[MenuItem("Make Cubemap/ Render Cubemap")]
	static void RenderCubemap(){
		
		ScriptableWizard.DisplayWizard ("Render CubeMap", typeof(GenerateCubeMap), "Render!");
		
	}
}