shader_type sky;

uniform vec3 foreground_color : source_color;
uniform vec3 background_color : source_color;
uniform sampler2D pattern : source_color;
uniform sampler2D pattern_2 : source_color;

float draw_curve(float y, float progress, float size, float fade_size) {
	float half_size = size * 0.5;
	return smoothstep(progress - fade_size, progress, y + half_size) - smoothstep(progress, progress + fade_size, y - half_size);
}

void sky() {
	if (AT_CUBEMAP_PASS) {
		COLOR = vec3(0.5);
	} else {
		float base_factor = texture(pattern, SCREEN_UV + TIME * 0.1).x;
		float noise_factor = texture(pattern_2, SCREEN_UV).x;
		
		
		COLOR = mix(background_color, foreground_color, mix(base_factor, noise_factor, 0.33));
	}
}
