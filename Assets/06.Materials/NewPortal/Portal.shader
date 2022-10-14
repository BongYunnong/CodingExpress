Shader "Custom/Portal"{
    Properties{
        [IntRange] _StencilID("Stencil ID",Range(0,255)) = 0
    }

    SubShader{
        Tags{
            // Portal이 다른 것보다 먼저 렌더링 되어야한다.
            // Geometry는 Queue가 2000임.
            "Queue" = "Geometry-1"
        }
        Pass{
            // 이 shader는 프레임 버퍼에 영향을 주지 않기에 ZBuffer를 사용할 필요가 없음
            Zwrite off
            // Portal 자체는 렌더링이 되면 안 됨 -> ColorMask 0
            ColorMask 0
            // 포탈의 앞면만 사용
            Cull front
            Stencil{
                // Stencil 버퍼를 사용하면 값을 1(or _StencilID)로 초기화
                Ref [_StencilID]
                // Stencil 버퍼에 뭐가 있든지 Pass 
                Comp always
                // 버퍼에 Ref 값을 적용
                Pass replace
            }
        }
    }
}
