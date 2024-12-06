
import { Home } from "./components/Home";
import {FirstScenarioBinary} from "./components/FirstScenarioBinary";
import {SecondScenarioText} from "./components/SecondScenarioText.tsx";
import {ThirdScenarioImage} from "./components/ThirdScenarioImage.tsx";
const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/first-scenario-binary',
    element: <FirstScenarioBinary />
  },
  {
    path: '/second-scenario-text',
    element: <SecondScenarioText />
  },
  {
    path: '/third-scenario-image',
    element: <ThirdScenarioImage />
  }
];

export default AppRoutes;
