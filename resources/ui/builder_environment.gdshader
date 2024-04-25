shader_type sky;

uniform vec3 foreground_color : source_color;
uniform vec3 background_color : source_color;
uniform sampler2D pattern : source_color;

float draw_curve(float y, float progress, float size, float fade_size) {
	float half_size = size * 0.5;
	return smoothstep(progress - fade_size, progress, y + half_size) - smoothstep(progress, progress + fade_size, y - half_size);
}

void sky() {
	if (AT_CUBEMAP_PASS) {
		COLOR = vec3(0.5);
	} else {
		float curve_point_1 = sin(SCREEN_UV.x * 2.0 * PI + TIME * 0.5) * 0.1 + 0.75;
		float curve_fade_1 = sin(TIME) * 0.01 + 0.015;
		float curve_1 = draw_curve(SCREEN_UV.y, curve_point_1, 0.1, curve_fade_1);
		
		float curve_point_2 = sin(SCREEN_UV.x * 2.0 * PI - TIME * 0.75) * 0.1 + 0.25;
		float curve_fade_2 = cos(TIME) * 0.01 + 0.015;
		float curve_2 = draw_curve(SCREEN_UV.y, curve_point_2, 0.1, curve_fade_2);
		
		COLOR = mix(background_color, foreground_color, clamp(curve_1 + curve_2, 0.0, 1.0));
	}
}
