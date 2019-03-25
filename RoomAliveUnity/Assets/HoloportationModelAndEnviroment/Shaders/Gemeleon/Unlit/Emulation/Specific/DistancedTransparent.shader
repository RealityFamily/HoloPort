// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33077,y:33022,varname:node_3138,prsc:2|emission-997-OUT,alpha-9029-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32613,y:32943,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.5019608,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_NormalVector,id:5717,x:32231,y:33104,prsc:2,pt:False;n:type:ShaderForge.SFN_ViewVector,id:9794,x:32231,y:33253,varname:node_9794,prsc:2;n:type:ShaderForge.SFN_Dot,id:1260,x:32436,y:33178,varname:node_1260,prsc:2,dt:1|A-5717-OUT,B-9794-OUT;n:type:ShaderForge.SFN_RemapRange,id:4296,x:32613,y:33178,varname:node_4296,prsc:2,frmn:0,frmx:1,tomn:0.4,tomx:1|IN-1260-OUT;n:type:ShaderForge.SFN_Multiply,id:997,x:32838,y:33044,varname:node_997,prsc:2|A-7241-RGB,B-4296-OUT;n:type:ShaderForge.SFN_Depth,id:7827,x:32613,y:33366,varname:node_7827,prsc:2;n:type:ShaderForge.SFN_Slider,id:370,x:32456,y:33521,ptovrint:False,ptlb:DistanceOffset,ptin:_DistanceOffset,varname:_DistanceOffset,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-2,cur:0,max:1;n:type:ShaderForge.SFN_Add,id:9029,x:32818,y:33366,varname:node_9029,prsc:2|A-7827-OUT,B-370-OUT;proporder:7241-370;pass:END;sub:END;*/

Shader "Gemeleon/Unlit/Emulation/Specific/DistancedTransparent" {
    Properties {
        _Color ("Color", Color) = (0,0.5019608,0.7843137,1)
        _DistanceOffset ("DistanceOffset", Range(-2, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _DistanceOffset;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 projPos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
////// Lighting:
////// Emissive:
                float3 emissive = (_Color.rgb*(max(0,dot(i.normalDir,viewDirection))*0.6+0.4));
                float3 finalColor = emissive;
                return fixed4(finalColor,(partZ+_DistanceOffset));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
