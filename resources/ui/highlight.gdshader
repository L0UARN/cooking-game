shader_type spatial;
render_mode unshaded;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;
uniform sampler2D normal_roughness_texture : hint_normal_roughness_texture, repeat_disable, filter_nearest;

const float SPACING = 0.9f;
const float OUTLINE_SIZE = 6.0f;

float nicer_sin(float x) {
	return sin(x) * 0.5 + 0.5;
}

float distance_to_edge(vec2 current_pos, vec2 current_pixel_pos, vec2 screen_size) {
	vec3 current_normal = texture(normal_roughness_texture, current_pos).xyz;
	
	for (float x = -OUTLINE_SIZE * 0.5f; x < OUTLINE_SIZE * 0.5f; x += 1.0f) {
		for (float y = -OUTLINE_SIZE * 0.5f; y < OUTLINE_SIZE * 0.5f; y += 1.0f) {
			vec2 test_pixel_pos = vec2(current_pixel_pos.x + x, current_pixel_pos.y + y);
			vec2 test_pos = test_pixel_pos / screen_size;
			vec3 test_normal = texture(normal_roughness_texture, test_pos).xyz;
			
			if (test_normal != current_normal) {
				return 1.0f;
			}
		}
	}
	
	return 0.0f;
}

void fragment() {
	vec3 previous_color = texture(screen_texture, SCREEN_UV).xyz;
	ivec2 i_screen_size = textureSize(screen_texture, 0);
	vec2 screen_size = vec2(float(i_screen_size.x), float(i_screen_size.y));
	float outline = distance_to_edge(SCREEN_UV, FRAGCOORD.xy, screen_size) * nicer_sin(TIME * PI) * 0.05f;
	
	float wave = nicer_sin(SCREEN_UV.x * 5.0f * PI - TIME * 2.0f);
	float spaced_wave = max(wave - SPACING, 0.0f) / SPACING;
	float smoothed_spaced_wave = smoothstep(0.0f, 1.0f, spaced_wave);
	float y = smoothed_spaced_wave + outline + 0.05f;
	
	ALBEDO = clamp(previous_color + vec3(y), vec3(0.0f), vec3(1.0f));
}
