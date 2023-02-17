local imgui = require 'imgui'

return function (time)
	imgui.Begin('Test')
	imgui.Text('Hello, world!')
	if imgui.Button('Hello, world!') then
		print('Hello, world!')
	end
	imgui.End()
end