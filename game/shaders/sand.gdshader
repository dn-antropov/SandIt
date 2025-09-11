shader_type canvas_item;
uniform sampler2D in_texture: source_color, filter_nearest;
uniform float particle_size : hint_range(1.0, 16.0) = 8.0;

float getCellBright(int id) {
    float fid = float(id);
    return sin(TIME+(mod(fid,16.)+1.)*2.)*.5+.5;
}

void fragment() {
    vec2 texture_size = vec2(textureSize(in_texture, 0));
    vec2 pixel_coord = UV * texture_size;
    
    // Check a radius around the current pixel
    COLOR = vec4(0.0, 0.0, 0.0, 0.0);
    
    float radius = particle_size * 0.5;
    for (float dy = -radius; dy <= radius; dy += 1.0) {
        for (float dx = -radius; dx <= radius; dx += 1.0) {
            vec2 sample_coord = pixel_coord + vec2(dx, dy);
            vec2 sample_uv = sample_coord / texture_size;
            
            if (sample_uv.x >= 0.0 && sample_uv.x <= 1.0 && sample_uv.y >= 0.0 && sample_uv.y <= 1.0) {
                vec4 texColor = texture(in_texture, sample_uv);
                int type = int(texColor.r * 255.0);
                int id = int(texColor.g * 255.0) + (int(texColor.b * 255.0) << 8);
                
                if (type > 0 && distance(vec2(dx, dy), vec2(0.0)) <= radius) {
                    if (type == 3) {
                        COLOR.rgb = vec3(1, 0, 0);
                    } else if (type == 1) {
                        COLOR.rgb = vec3(0.25, 0.25, 0.25);
                    } else {
                        float randBright = getCellBright(id);
                        vec3 neonGreen = vec3(15./255., 1., 80./255.);
                        vec3 color = neonGreen * vec3(randBright)*0.25 + 0.5;
                        color = mix(vec3(0), color, min(1.,float(type)));
                        COLOR.rgb = color.rgb;
                    }
                    COLOR.a = 1.0;
                    break;
                }
            }
        }
    }
}