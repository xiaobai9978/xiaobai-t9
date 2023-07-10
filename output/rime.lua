function time_translator(input, seg)
   if (input == "1414") then
      local cand = Candidate("date", seg.start, seg._end, os.date("%Y/%m/%d"), "")
      cand.quality = 1
      yield(cand)
   end
   if (input == "1445486") then
      local cand = Candidate("time", seg.start, seg._end, os.date("%H:%M"), " ")
      cand.quality = 1
      yield(cand)
   end
end