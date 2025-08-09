shader_type canvas_item;

uniform sampler2D in_texture: source_color, filter_nearest;
uniform float speed;

float rand(vec2 co){ return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453); } // random noise

float getCellBright(vec2 id) {
    return sin((TIME*speed)*rand(id)*2.)*.5+.5; // returns 0. to 1.
}

void fragment() {
	vec4 color = texture(in_texture, UV);
	vec2 id = floor(UV*256.);

	float randBright = getCellBright(id);
	COLOR.rgb = color.rgb * randBright ;
}
