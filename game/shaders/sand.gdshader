shader_type canvas_item;

uniform sampler2D in_texture: source_color, filter_nearest;

float getCellBright(int id) {
	float fid = float(id);
    return sin(TIME+(mod(fid,16.)+1.)*2.)*.5+.5; // returns 0. to 1.
}

void fragment() {
	//vec4 texColor = texture(in_texture, UV);
	//int type = int(texColor.r * 255.0);
	//int id = int(texColor.g * 255.0) + (int(texColor.b * 255.0) << 8);
//
	//if (type == 3) {
		//COLOR.rgb = vec3(1, 0, 0);
	//} else if (type == 1) {
		//COLOR.rgb = vec3(0.25, 0.25, 0.25);
	//} else {
		//float randBright = getCellBright(id);
		//vec3 neonGreen = vec3(15./255., 1., 80./255.);
		//vec3 color = neonGreen * vec3(randBright)*0.25 + 0.5;
		//color = mix(vec3(0), color, min(1.,float(type)));
		//COLOR.rgb = color.rgb;
	//}
	COLOR.rgba = vec4(0);
}