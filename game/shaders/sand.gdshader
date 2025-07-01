shader_type canvas_item;

uniform sampler2D in_texture: source_color, filter_nearest;
uniform float fade_speed;

void fragment() {
	vec4 color = texture(in_texture, UV);
	float fade = 1.0;
	if (color.r > 0.0) 
		fade = sin(TIME * fade_speed) * 0.5 + 0.5;
	COLOR.rgb = color.rgb * fade;
}
