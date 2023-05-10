import AutoLabeller from "./scenes/AutoLabeller/AutoLabeller";
import ChatLabeller from "./scenes/ChatLabeller/ChatLabeller";
import GenerativeLabeller from "./scenes/GenerativeLabeller/GenerativeLabeller";

const AppRoutes = [
  {
    path: "/chat-labeller",
    element: <ChatLabeller />
  },
  {
    path: "/chat-labeller/:chatId",
    element: <ChatLabeller/>
  },
  {
    path: "/auto-labeller",
    element: <AutoLabeller/>,
  },
  {
    path: "/auto-labeller/:originalSourceId",
    element: <AutoLabeller/>
  },
  {
    path: '/generative-labeller',
    element: <GenerativeLabeller />
  },
  {
    path: '/generative-labeller/:sampleId',
    element: <GenerativeLabeller />
  }
];

export default AppRoutes;
