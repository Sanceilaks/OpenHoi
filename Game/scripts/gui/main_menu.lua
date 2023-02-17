local imgui = require 'imgui'

return function (time)
	imgui.Begin('Test')
	imgui.Text('Hello, world!')
	imgui.End()
end