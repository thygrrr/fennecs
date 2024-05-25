extends Line2D

var max_points : int = 50
const skip := 2
var frame := 0

func _physics_process(delta: float) -> void:
	frame += 1
	var pos = get_parent().global_position
	if frame % skip == 0:
		while get_point_count() > max_points:
			remove_point(0)
		add_point(pos)
	else:
		set_point_position(-1, pos)
