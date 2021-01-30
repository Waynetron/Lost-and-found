Shader "Custom/ISOheight contour"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		 _Sea("Sea",Color) = (1,1,1,1)
        _BaseColor("BaseColor",Color) = (1,1,1,1)
        _LineColor("LineColor",Color) = (1,1,1,1)
                 _HeightLineRate("HeightLineRate",Range(0,1)) = 1//The proportion of the contour line to HeightOffset
                 _HeightOffset("HeightOffset",Float) = 10//Height segment
				 _Waterheight("sealine",Float) = 5
				 
     

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
               
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;

                float4 vertex : SV_POSITION;
                float3 worldPos:TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

			 fixed4 _Sea;
            fixed4 _BaseColor;
            fixed4 _LineColor;
            float _HeightOffset;
            float _HeightLineRate;
			float _Waterheight;
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col;
				if (i.worldPos.y<_Waterheight){
                col = tex2D(_MainTex, i.uv) *_Sea  ;
				}
				else
				{
				  col = tex2D(_MainTex, i.uv) * _BaseColor ;
				  }
                                 fixed tempRate = i.worldPos.y / _HeightOffset;//The current pixel height divided by HeightOffset
                                 fixed fract = tempRate-floor(tempRate);//Take the decimal part to get the scale value (fact is not recognized in the unity shader, so use this method)
                                 fixed funRes =1- step(fract,1 -_HeightLineRate);//Calculate whether the current height is within the ratio of U2, is it 1, not 0
                fixed4 fincolor = col * (1-funRes) + (_LineColor*i.worldPos.y ) * funRes ;
                return fincolor;
            }
            ENDCG
        }
    }
}

