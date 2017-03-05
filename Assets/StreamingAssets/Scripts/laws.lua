function test()
	print("Hello!")
	return true
end

function basicIncomeTaxOnPropose()
	-- print("basicIncomeTaxOnPropose!")
end

function basicIncomeTaxOnEnact()
	-- print("basicIncomeTaxOnEnact!")
end

function basicIncomeTaxOnRegionTic(law, region)
	-- print("LUA:" .. law.id .. region[1].name)
	-- print(law.getParameterAsInt("_cost"))
	-- print(region.country.economy.funds)
	region.country.economy.funds = law.getParameterAsInt("_cost") + region.country.economy.funds
end

function basicIncomeTaxOnTic()
	-- print("basicIncomeTaxOnTic!")
end