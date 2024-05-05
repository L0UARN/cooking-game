shader_type canvas_item;

uniform float radius;

void fragment() {
	vec2 resolution = 1.0 / SCREEN_PIXEL_SIZE;
	
	if (resolution.x > resolution.y) {
		float aspect_ratio = resolution.y / resolution.x;
		float circle = step(radius, length(vec2(SCREEN_UV.x, SCREEN_UV.y * aspect_ratio) - vec2(0.5, 0.5 * aspect_ratio)));
		COLOR = vec4(vec3(1.0 - circle), circle);
	} else {
		float aspect_ratio = resolution.x / resolution.y;
		float circle = step(radius, length(vec2(SCREEN_UV.x * aspect_ratio, SCREEN_UV.y) - vec2(0.5 * aspect_ratio, 0.5)));
		COLOR = vec4(vec3(1.0 - circle), circle);
	}
}
