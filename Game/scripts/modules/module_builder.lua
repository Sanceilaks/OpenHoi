local sc = {}
sc.__index = sc

local function new()
	local self = {
		name = nil,
		starts_at = nil,
		description = nil,
		author = nil,
	}
	return setmetatable(self, sc)
end

function sc.set_name(self, name)
	self.name = name
	return self
end

function sc.set_description(self, desc)
	self.description = desc
	return self
end

function sc.set_author(self, author)
	self.author = author
	return self
end

function sc.set_start_date(self, starts_at)
	self.starts_at = starts_at
	return self
end

return {
	new = new,
	__object = sc
}