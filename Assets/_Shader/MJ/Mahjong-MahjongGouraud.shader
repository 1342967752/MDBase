Shader "Mahjong/MahjongGouraud" {
Properties {
 _Color ("Main Color", Color) = (0,0,0,1)
 _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
 _LightDir ("LightDir", Vector) = (0.5,0.5,0.5,1)
 _LightDir1 ("LightDir1", Vector) = (0.5,0.5,0.5,1)
 _Shininess ("Shininess", Range(0.01,50)) = 0.078125
 _Emission ("Emission", Range(0.01,1)) = 0.3
 _MainTex ("Base (RGB) RefStrGloss (A)", 2D) = "white" {}
 _Mask ("MaskTexture", 2D) = "white" {}
}
SubShader { 
 Pass {
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 glstate_lightmodel_ambient;
uniform lowp vec4 _Color;
uniform lowp vec3 _LightDir1;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec3 ambientLighting_1;
  mediump vec3 normalDir_2;
  lowp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4.w = 0.0;
  tmpvar_4.xyz = normalize(_glesNormal);
  highp vec3 tmpvar_5;
  tmpvar_5 = normalize((tmpvar_4 * _World2Object).xyz);
  normalDir_2 = tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_6 = (glstate_lightmodel_ambient.xyz * _Color.xyz);
  ambientLighting_1 = tmpvar_6;
  lowp vec3 cse_7;
  cse_7 = normalize(_LightDir1);
  lowp float tmpvar_8;
  tmpvar_8 = sqrt(dot (cse_7, cse_7));
  mediump vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = (ambientLighting_1 + ((
    (1.0/(tmpvar_8))
   * _Color.xyz) * max (0.0, 
    dot (normalDir_2, cse_7)
  )));
  mediump vec4 tmpvar_10;
  tmpvar_10 = clamp (tmpvar_9, vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0));
  tmpvar_3 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform mediump float _Emission;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * (xlv_TEXCOORD1 + _Emission));
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 glstate_lightmodel_ambient;
uniform lowp vec4 _Color;
uniform lowp vec3 _LightDir1;
out highp vec2 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec3 ambientLighting_1;
  mediump vec3 normalDir_2;
  lowp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4.w = 0.0;
  tmpvar_4.xyz = normalize(_glesNormal);
  highp vec3 tmpvar_5;
  tmpvar_5 = normalize((tmpvar_4 * _World2Object).xyz);
  normalDir_2 = tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_6 = (glstate_lightmodel_ambient.xyz * _Color.xyz);
  ambientLighting_1 = tmpvar_6;
  lowp vec3 cse_7;
  cse_7 = normalize(_LightDir1);
  lowp float tmpvar_8;
  tmpvar_8 = sqrt(dot (cse_7, cse_7));
  mediump vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = (ambientLighting_1 + ((
    (1.0/(tmpvar_8))
   * _Color.xyz) * max (0.0, 
    dot (normalDir_2, cse_7)
  )));
  mediump vec4 tmpvar_10;
  tmpvar_10 = clamp (tmpvar_9, vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0));
  tmpvar_3 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_3;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform mediump float _Emission;
in highp vec2 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture (_MainTex, xlv_TEXCOORD0) * (xlv_TEXCOORD1 + _Emission));
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
SubProgram "gles3 " {
"!!GLES3"
}
}
 }
}
Fallback "Unlit/Texture"
}