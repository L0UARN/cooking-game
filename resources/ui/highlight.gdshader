shader_type spatial;
render_mode unshaded;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

const float SPACING = 0.9f;

float nicer_sin(float x) {
	return sin(x) * 0.5 + 0.5;
}

void fragment() {
	float wave_y = max(sin(SCREEN_UV.y * PI * 10.0f + TIME * 2.0f), 0.0f);
	float smooth_wave = smoothstep(0.0f, 1.0f, wave_y);
	float y = smooth_wave * 0.15f + 0.05f;
	
	vec3 previous_color = texture(screen_texture, SCREEN_UV).xyz;
	ALBEDO = clamp(previous_color + vec3(y), vec3(0.0f), vec3(1.0f));
}
