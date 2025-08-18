shader_type canvas_item;

uniform sampler2D in_texture: source_color, filter_nearest;

float getCellBright(float id) {
    return sin(TIME*(mod(id,3.)+1.)*2.)*.5+.5; // returns 0. to 1.
}

void fragment() {
	vec4 color = texture(in_texture, UV);
	float id = color.g * color.b * 16.384;
	float type = color.r;
	float randBright = getCellBright(id);
	COLOR.rgb = mix(vec3(0), vec3(127. * randBright) + vec3(127), type) ;
}
