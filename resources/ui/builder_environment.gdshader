shader_type sky;

uniform sampler2D pattern : source_color;
uniform vec3 background_color : source_color;
uniform vec3 foreground_color : source_color;

void sky() {
	if (AT_CUBEMAP_PASS) {
		COLOR = mix(vec3(0.5), vec3(1.0), EYEDIR.y);
	} else {
		vec2 screen_size = FRAGCOORD.xy / SCREEN_UV;
		vec2 texture_size = vec2(textureSize(pattern, 0));
		float texture_ratio = texture_size.y / texture_size.x;
		vec2 new_texture_size = vec2(screen_size.y, screen_size.y * texture_ratio);

		vec2 texture_uv = FRAGCOORD.xy / new_texture_size;
		vec2 corrected_screen_uv = vec2(texture_uv.x, SCREEN_UV.y);
		vec4 pattern_color = texture(pattern, corrected_screen_uv * 4.0 + TIME * 0.1);
		float pattern_level = pattern_color.a;

		COLOR = vec3(mix(background_color, foreground_color, pattern_level));
	}
}
