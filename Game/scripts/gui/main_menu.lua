local imgui = require 'imgui'

return function (time)
	imgui.Begin('Test')
	imgui.Text('Hello, world!')
	if imgui.Button('Hello, world!') then
		print('Hello, world!')
	end

	if imgui.BeginTable("Test Table", 3) then
		imgui.TableSetupColumn("Column 1")
		imgui.TableSetupColumn("Column 2")
		imgui.TableSetupColumn("Column 3")
		imgui.TableHeadersRow();
		imgui.EndTable();
	end

	if imgui.Button('Hello, world!') then
		print('Hello, world!')
	end

	imgui.Text(tostring(Game.Modifications[0].Name))

	imgui.End()
end