// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33229,y:32796,varname:node_3138,prsc:2|emission-377-OUT,alpha-7854-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32221,y:32408,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:0.25;n:type:ShaderForge.SFN_Tex2d,id:2860,x:32221,y:32738,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3545-UVOUT;n:type:ShaderForge.SFN_NormalVector,id:7372,x:31834,y:33172,prsc:2,pt:False;n:type:ShaderForge.SFN_ViewVector,id:4096,x:31834,y:33320,varname:node_4096,prsc:2;n:type:ShaderForge.SFN_Dot,id:2547,x:32036,y:33240,varname:node_2547,prsc:2,dt:1|A-7372-OUT,B-4096-OUT;n:type:ShaderForge.SFN_RemapRange,id:4636,x:32221,y:33240,varname:node_4636,prsc:2,frmn:0,frmx:1,tomn:0.4,tomx:1|IN-2547-OUT;n:type:ShaderForge.SFN_Multiply,id:4355,x:32597,y:32795,varname:node_4355,prsc:2|A-1196-OUT,B-2860-RGB,C-9289-RGB,D-4636-OUT;n:type:ShaderForge.SFN_Tex2d,id:9289,x:32221,y:32985,ptovrint:False,ptlb:DetailTex,ptin:_DetailTex,varname:_DetailTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-199-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:199,x:32036,y:32985,varname:node_199,prsc:2,uv:1;n:type:ShaderForge.SFN_TexCoord,id:3545,x:32036,y:32738,varname:node_3545,prsc:2,uv:0;n:type:ShaderForge.SFN_Color,id:9450,x:32597,y:32962,ptovrint:False,ptlb:HighlightColor,ptin:_HighlightColor,varname:_HighlightColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:0,c4:0;n:type:ShaderForge.SFN_Lerp,id:377,x:32842,y:32884,varname:node_377,prsc:2|A-4355-OUT,B-9450-RGB,T-9450-A;n:type:ShaderForge.SFN_Set,id:8040,x:32419,y:32507,varname:Alpha,prsc:2|IN-7241-A;n:type:ShaderForge.SFN_Get,id:9640,x:32821,y:33044,varname:node_9640,prsc:2|IN-8040-OUT;n:type:ShaderForge.SFN_Set,id:2180,x:32419,y:32402,varname:Color,prsc:2|IN-7241-RGB;n:type:ShaderForge.SFN_Get,id:1196,x:32200,y:32636,varname:node_1196,prsc:2|IN-2180-OUT;n:type:ShaderForge.SFN_Lerp,id:7854,x:33017,y:33064,varname:node_7854,prsc:2|A-9640-OUT,B-6555-OUT,T-9450-A;n:type:ShaderForge.SFN_Vector1,id:6555,x:32842,y:33108,varname:node_6555,prsc:2,v1:1;proporder:7241-2860-9289-9450;pass:END;sub:END;*/

Shader "Gemeleon/Unlit/Emulation/Building/Transparent" {
    Properties {
        _Color ("Color", Color) = (1,1,1,0.25)
        _MainTex ("MainTex", 2D) = "white" {}
        _DetailTex ("DetailTex", 2D) = "white" {}
        _HighlightColor ("HighlightColor", Color) = (1,1,0,0)
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _DetailTex; uniform float4 _DetailTex_ST;
            uniform float4 _HighlightColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 Color = _Color.rgb;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _DetailTex_var = tex2D(_DetailTex,TRANSFORM_TEX(i.uv1, _DetailTex));
                float3 emissive = lerp((Color*_MainTex_var.rgb*_DetailTex_var.rgb*(max(0,dot(i.normalDir,viewDirection))*0.6+0.4)),_HighlightColor.rgb,_HighlightColor.a);
                float3 finalColor = emissive;
                float Alpha = _Color.a;
                return fixed4(finalColor,lerp(Alpha,1.0,_HighlightColor.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
