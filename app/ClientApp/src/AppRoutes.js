import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import {FirstScenarioBinary} from "./components/FirstScenarioBinary";
import {SecondScenarioText} from "./components/SecondScenarioText.tsx";
const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/first-scenario-binary',
    element: <FirstScenarioBinary />
  },
  {
    path: '/second-scenario-text',
    element: <SecondScenarioText />
  }
];

export default AppRoutes;
