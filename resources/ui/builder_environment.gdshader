shader_type sky;

uniform sampler3D noise_texture : source_color;

float nicer_sin(float x) {
	return (sin(x * PI) + sin(x * PI * 4.0)) * 0.24 + 0.5;
}

void sky() {
	if (AT_CUBEMAP_PASS) {
		COLOR = vec3(EYEDIR.y * 0.5 + 0.5) * 0.5;
	} else {
		vec4 background = texture(noise_texture, vec3(SCREEN_UV, nicer_sin(TIME * PI * 0.01)));
		COLOR = background.xyz;
	}
}
